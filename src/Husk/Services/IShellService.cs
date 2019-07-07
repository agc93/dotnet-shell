using System.Diagnostics;

namespace Husk.Services
{
    public interface IShellService
    {
        int SpawnShell(string shellExecutable, string arguments = null);
    }

    public class BasicShellService : IShellService
    {
        public int SpawnShell(string shellExecutable, string arguments = null)
        {
            var process = new Process();
            process.StartInfo.FileName = shellExecutable;
            process.StartInfo.Arguments = arguments ?? string.Empty;
            process.StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
            try {
                System.Console.Clear();
                System.Console.Title = System.IO.Path.GetFileNameWithoutExtension(shellExecutable);
            } catch {
                // ignored
            }
            var result = process.Start();
            if (result) {
                process.WaitForExit();
                return process.Id;
            }
            return -1;
        }
    }
}