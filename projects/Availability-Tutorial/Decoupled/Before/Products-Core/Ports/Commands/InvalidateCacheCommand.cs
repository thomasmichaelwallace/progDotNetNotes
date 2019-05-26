using System;
using paramore.brighter.commandprocessor;

namespace Products_Core.Ports.Commands
{
    public class InvalidateCacheCommand : Command
    {
        /// <summary>
        /// Gets the resource to invalidate.
        /// </summary>
        /// <value>The resource to invalidate.</value>
        public Uri ResourceToInvalidate { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command" /> class.
        /// </summary>
        /// <param name="resourceToInvalidate">The resource to invalidate.</param>
        public InvalidateCacheCommand(Uri resourceToInvalidate) : base(Guid.NewGuid())
        {
            this.ResourceToInvalidate = resourceToInvalidate;
        }
    }
}
