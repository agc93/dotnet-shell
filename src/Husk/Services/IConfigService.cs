using System.IO;
using static System.Environment;

namespace Husk.Services
{
    public interface IConfigService
    {
         bool WriteConfig(ShellCollection shells);
         ShellCollection ReadConfig();
    }

    public class YamlConfigService : IConfigService
    {
        private string GetConfigFile() {
            var path = Path.Combine(GetFolderPath(SpecialFolder.UserProfile), "shells.yaml");
            if (File.Exists(path)) {
                return path;
            } else {
                File.Create(path).Close();
                return path;
            }
        }

        public ShellCollection ReadConfig()
        {
            var file = GetConfigFile();
            var text = File.ReadAllText(file);
            return file.ToShellCollection();
        }

        public bool WriteConfig(ShellCollection shells)
        {
            var file = GetConfigFile();
            var text = shells.ToYaml();
            File.WriteAllText(file, text);
            return File.Exists(file);
        }
    }
}