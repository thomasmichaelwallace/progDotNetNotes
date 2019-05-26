using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products_Core.Ports.Caching
{
    public interface IAmACache
    {
        /// <summary>
        /// Invalidates the resource.
        /// </summary>
        /// <param name="resourceToInvalidate">The resource to invalidate.</param>
        void InvalidateResource(Uri resourceToInvalidate);
    }
}
