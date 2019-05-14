using Husk.Services;
using Spectre.Cli;

namespace Husk
{
    public class StartCommand : Command<StartSettings>
    {
        private readonly IShellDiscoveryService _discoveryService;
        private readonly IShellService _shellService;

        public StartCommand(IShellDiscoveryService discoveryService, IShellService shellService)
        {
            _discoveryService = discoveryService;
            _shellService = shellService;
        }
        public override int Execute(CommandContext context, StartSettings settings)
        {
            return 0;
        }
    }
}