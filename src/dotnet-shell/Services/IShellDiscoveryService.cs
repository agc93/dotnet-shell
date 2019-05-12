using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dotnet_shell.Services
{
    public interface IShellDiscoveryService
    {
         ShellCollection FindShells();
    }

    public class DefaultShellDiscoveryService : IShellDiscoveryService
    {
        public ShellCollection FindShells()
        {
            switch (Environment.OSVersion.Platform) {
                case PlatformID.Win32NT:
                    return GetWindowsShells();
                case PlatformID.Unix:
                    return GetUnixShells();
                case PlatformID.MacOSX:
                    return GetMacOsShells();
                default:
                    return new ShellCollection();
            }
        }

        private ShellCollection GetMacOsShells()
        {
            System.Console.WriteLine("WARNING: Automatic shell detection is not currently supported on macOS. Falling back to bash...");
            return new ShellCollection{ ["bash"] = "bash" };
        }

        private ShellCollection GetUnixShells()
        {
            try
            {
                return File.ReadAllLines("/etc/shells")
                    .Where(l => !l.StartsWith("#"))
                    .Select(l => new KeyValuePair<string, string>(l.Split('/').Last(), l))
                    .Reverse()
                    .ToShellCollection();
            }
            catch
            {
                return new ShellCollection() { ["bash"] = "bash" };
            }
        }

        private ShellCollection GetWindowsShells()
        {
            var shells = new ShellCollection {
                ["cmd"] = "cmd.exe",
                ["powershell"] = "powershell.exe"
            };
            if (Environment.GetEnvironmentVariable("PATH").Split(";").Any(p => File.Exists(Path.Combine(p, "wsl.exe")))) {
                shells.AddShell("bash", "wsl.exe");
            }
            return shells;
        }
    }
}