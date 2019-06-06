using System.ComponentModel;
using Spectre.Cli;

namespace Husk
{
    public class ConfigSettings : ShellSettings
    {
        [CommandOption("-f|--file")]
        [Description("Override the default config file location. Defaults to ~/.shells.yaml")]
        public string FilePath {get;set;}

        public class ConfigListSettings : ConfigSettings {
            [CommandOption("--pretty")]
            public bool UsePrettyOutput {get;set;}
        }

        public class ConfigCreateSettings : ConfigSettings {
            [CommandOption("--force")]
            [Description("Forces the creation of the config, potentially overwriting existing configuration")]
            public bool Force {get;set;}
        }
    }
}