using ConsoleTables;
using static System.Console;
using Husk.Services;
using Spectre.Cli;
using System.ComponentModel;
using System;

namespace Husk
{
    namespace Config {
        public abstract class ConfigCommand<T> : Command<T> where T : ConfigSettings {

            protected IConfigService ConfigService { get; }
            public ConfigCommand(IConfigService configService)
            {
                ConfigService = configService;
            }

            /// <summary>
            /// Gets the shell config from the underlying config service
            /// </summary>
            /// <param name="filePath"></param>
            /// <returns></returns>
            /// <remarks>Is this method basically useless? yes. Did it make way more sense with a previous iteration of this code? also yes.</remarks>
            protected ShellCollection GetConfig(string filePath) {
                var config = ConfigService.ReadConfig(filePath);
                return config;
            }
        }

        [Description("Lists available shells from the current configuration.")]
        public class ConfigListCommand : ConfigCommand<ConfigSettings.ConfigListSettings>
        {
            public ConfigListCommand(IConfigService configService) : base(configService)
            {
            }

            public override int Execute(CommandContext context, ConfigSettings.ConfigListSettings settings)
            {
                var config = GetConfig(settings.FilePath).AddShells(settings.ExtraShell);
                if (settings.UsePrettyOutput) {
                    var table = new ConsoleTable("Shell", "Path");
                    foreach (var shell in config)
                    {
                        table.AddRow(shell.Key, shell.Value);
                    }
                    table.Write(Format.Minimal);
                    return 200;
                }
                foreach (var shell in config)
                {
                    WriteLine($"{shell.Key}={shell.Value}");
                }
                return 200;
            }
        }

        [Description("Creates a new configuration file based on automatically detected shells.")]
        public class ConfigCreateCommand : ConfigCommand<ConfigSettings.ConfigCreateSettings>
        {
            public ConfigCreateCommand(IShellDiscoveryService discoveryService, IConfigService configService) : base(configService)
            {
                DiscoveryService = discoveryService;
            }

            private IShellDiscoveryService DiscoveryService { get; }

            public override int Execute(CommandContext context, ConfigSettings.ConfigCreateSettings settings)
            {
                var shells = DiscoveryService.FindShells().AddShells(settings.ExtraShell);
                var config = GetConfig(settings.FilePath);
                if ((config.Count != 0 && !settings.Force)) {
                    WriteLine("WARNING: Existing configuration found! Pass --force to overwrite existing configuration.");
                    return 412;
                } else {
                    ConfigService.WriteConfig(shells);
                    return 201;
                }
            }
        }

        [Description("Completely removes the configuration, essentially resetting Husk to defaults.")]
        public class ConfigDeleteCommand : ConfigCommand<ConfigSettings>
        {
            public ConfigDeleteCommand(IConfigService configService) : base(configService)
            {
            }

            public override int Execute(CommandContext context, ConfigSettings settings)
            {
                if (string.IsNullOrWhiteSpace(settings.FilePath)) {
                    return RemoveConfig(() => ConfigService.WriteConfig(new ShellCollection()));
                } else {
                    if (System.IO.File.Exists(settings.FilePath)) {
                        return RemoveConfig(() => System.IO.File.Delete(settings.FilePath), $"Are you sure you want to *permanently* remove the configuration from '{settings.FilePath}'?");
                    } else {
                        WriteLine("Config file not found!");
                        return 404;
                    }
                }
            }

            private int RemoveConfig(Action deleteAction, string warning = null) {
                WriteLine("This action cannot be undone!");
                var response = InquirerCS.Question.Confirm(string.IsNullOrWhiteSpace(warning) ? "Are you sure you want to remove the configuration? (Cannot be undone!)" : warning).Prompt();
                if (response) {
                    deleteAction.Invoke();
                    return 410;
                } else {
                    return 304;
                }

            }
        }
    }
}