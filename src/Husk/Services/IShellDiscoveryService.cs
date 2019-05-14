using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Husk.Services
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
            /*
            macOS is a tricky edge case here.
            macOS does actually support /etc/shells, but as far as I can tell this only applies to weird system cases like ftp,
            and does not represent available user shells. Shells are now changed using Directory Services and `dscl` which is too 
            complicated for me, especially without a Mac to test with.
            Pretty sure zsh is default installed, so including that.
             */
            System.Console.WriteLine("WARNING: Automatic shell detection is not currently supported on macOS. Falling back to defaults...");
            return new ShellCollection{ 
                ["bash"] = "bash",
                ["zsh"] = "zsh"
            };
        }

        /// <summary>
        /// Gets a collection of avialable shells for a Linux environment
        /// </summary>
        /// <returns>A collection of available shells.</returns>
        /// <remarks>
        /// Linux is a bit easier since we can just check /etc/shells for installed shells. 
        /// This is often quite noisy (Ubuntu and Fedora have ~4) but there's not much we can do of semantic value here. 
        /// </remarks>
        private ShellCollection GetUnixShells()
        {
            try
            {
                return File.ReadAllLines("/etc/shells")
                    .Where(l => !l.StartsWith("#")) // in case someone is commenting /etc/shells for some reason // which as it turns out Ubuntu does ffs.
                    .Where(l => !l.Contains("nologin")) // strip out nologin shells
                    .Select(l => new KeyValuePair<string, string>(l.Split('/').Last(), l)) //use executable name as shell name
                    // .Reverse() // this sort of made sense if it was using PATH semantics, but its not
                    .ToShellCollection();
            }
            catch
            {
                // we should probably change this to `sh` realistically
                return new ShellCollection() { ["bash"] = "bash" }; // everyone has bash, or at least something aliased to it.
            }
        }

        /// <summary>
        /// Gets a collection of available shells for a Windows environment
        /// </summary>
        /// <returns>A collection of available shells</returns>
        /// <remarks>
        /// Windows is tricky since it doesn't have any native way to list available shells AFAIK. 
        /// To make up for it, we're just going to assume cmd and powershell are available. 
        /// Then, we can check if wsl.exe is on PATH and add that as an option.
        /// </remarks>
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