using System;
using Husk.Services;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Cli;
using Terminal.Gui;

namespace Husk
{
    class Program
    {
        static int Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddSingleton<IShellService, BasicShellService>()
                .AddSingleton<IShellDiscoveryService, DefaultShellDiscoveryService>()
                .AddSingleton<IConfigService, YamlConfigService>();
            var app = new CommandApp(new ServiceRegistrar(services));
            app.Configure(config => {
                config.AddBranch<ShellSettings>("config", branch => {
                    branch.SetDescription("Extra commands for working with the Husk configuration file");
                    branch.AddCommand<Config.ConfigCreateCommand>("create");
                    branch.AddCommand<Config.ConfigListCommand>("list");
                    branch.AddCommand<Config.ConfigDeleteCommand>("delete");
                });
                config.AddCommand<MenuCommand>("menu");
            });
            app.SetDefaultCommand<MenuCommand>();
            return app.Run(args);
        }
    }
}
