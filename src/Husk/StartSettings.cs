using System.ComponentModel;
using Spectre.Cli;

namespace Husk
{
    public class StartSettings : ShellSettings
    {
        [CommandArgument(0, "<shell>")]
        [Description("The shell to start.")]
        public string ShellName { get; set; } = string.Empty;
    }
}