using System.ComponentModel;
using Spectre.Cli;

namespace dotnet_shell
{
    public class ShellSettings : CommandSettings
    {
        [CommandOption("--shell")]
        [Description("Adds an extra shell to the available options.")]
        public string[] ExtraShell {get;set;} = new string[0];
    }
}