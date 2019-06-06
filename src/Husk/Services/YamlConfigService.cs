using System.IO;
using static System.Environment;

namespace Husk.Services
{
    public class YamlConfigService : IConfigService
    {
        private string GetConfigFile(bool createIfNotExists = false) {
            var localPath = Path.Combine(CurrentDirectory, ".shells.yaml");
            if (File.Exists(localPath)) return localPath; // this is intended only as an "override" capability, not as a normal way of configuring shells.
            var path = Path.Combine(GetFolderPath(SpecialFolder.UserProfile), ".shells.yaml");
            if (File.Exists(path)) {
                return path;
            } else {
                if (createIfNotExists) File.Create(path).Close();
                return path;
            }
        }

        public ShellCollection ReadConfig(string filePath = null)
        {
            var file = (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath)) ? filePath : GetConfigFile();
            var text = File.Exists(file) ? File.ReadAllText(file) : string.Empty;
            return string.IsNullOrWhiteSpace(text) ? new ShellCollection() : text.ToShellCollection();
        }

        public bool WriteConfig(ShellCollection shells)
        {
            var file = GetConfigFile(createIfNotExists: true);
            var text = shells.ToYaml();
            File.WriteAllText(file, text);
            return File.Exists(file);
        }
    }
}