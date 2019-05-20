using System.ComponentModel;
using Spectre.Cli;

namespace Husk
{
    public class MenuSettings : ShellSettings
    {
        [CommandOption("--auto")]
        [Description("Enables auto-discovering default shells available on the current host. May be slightly slower.")]
        public bool EnableDiscovery {get;set;}

        [CommandOption("-l|--loop")]
        [Description("Return to the shell selection menu when you exit your current shell.")]
        public bool LoopShells {get;set;}

        [CommandOption("-v|--verbose")]
        [Description("Include the executable path for the shell in the menu.")]
        public bool IncludePath {get;set;}
        
    }
}