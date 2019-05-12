using System.ComponentModel;
using Spectre.Cli;

namespace dotnet_shell
{
    public class MenuSettings : ShellSettings
    {
        [CommandOption("--auto")]
        [Description("Enables auto-discovering default shells available on the current host. May be slightly slower.")]
        public bool EnableDiscovery {get;set;}

        [CommandOption("-l|--loop")]
        [Description("Return to the shell selection menu when you exit your current shell.")]
        public bool LoopShells {get;set;}
        
    }
}