using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Husk.Services;
using InquirerCS;
using Spectre.Cli;
using Terminal.Gui;

namespace Husk
{
    [Description("Invoke a menu to select your shell interactively")]
    public class MenuCommand : Command<MenuSettings>
    {
        public MenuCommand(IShellService shellService, IShellDiscoveryService discoveryService)
        {
            ShellService = shellService;
            DiscoveryService = discoveryService;
        }

        private IShellService ShellService { get; }
        private IShellDiscoveryService DiscoveryService { get; }

        public override int Execute(CommandContext context, MenuSettings settings)
        {
            int id = 0;
            ShellCollection shells = settings.EnableDiscovery ? DiscoveryService.FindShells() : new ShellCollection();
            settings.ExtraShell.ToList().ForEach(s => shells.AddShell(System.IO.Path.GetFileNameWithoutExtension(s), s));
            if (shells.Count < 1) {
                System.Console.WriteLine("No shells available! Configure your shells, use --auto, or provide a shell with --shell to continue.");
                System.Console.ReadLine();
                return 424;
            }
            var menu = Question.Menu("Choose Shell...");
            foreach (KeyValuePair<string, string> shell in shells)
            {
                menu.AddOption(shell.Key, () => id = ShellService.SpawnShell(shell.Value));
            }
            if (settings.LoopShells) {
                menu.AddOption("-Exit", () => id = -1);
            }
            while (id != -1)
            {
                menu.Prompt();
                if (!settings.LoopShells) {
                    id = -1;
                }
            }
            return id;
        }
    }
}