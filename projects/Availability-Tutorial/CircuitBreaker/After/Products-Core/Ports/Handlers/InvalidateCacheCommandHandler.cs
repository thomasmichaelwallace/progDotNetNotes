using paramore.brighter.commandprocessor;
using Products_Core.Ports.Caching;
using Products_Core.Ports.Commands;

namespace Products_Core.Ports.Handlers
{
    public class InvalidateCacheCommandHandler : RequestHandler<InvalidateCacheCommand>
    {
        private IAmACache _cache;

        public InvalidateCacheCommandHandler(IAmACache cache)
        {
            _cache = cache;
        }

        public override InvalidateCacheCommand Handle(InvalidateCacheCommand command)
        {
            _cache.InvalidateResource(command.ResourceToInvalidate);
            return base.Handle(command);
        }
    }
}
