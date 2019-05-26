using System;
using System.Net.Http;
using CacheCow.Server;
using Products_Core.Ports.Caching;

namespace Product_API.Adapters.Caching
{
    public class Cache : IAmACache
    {
        private readonly ICachingHandler _cachingHandler;

        public Cache(ICachingHandler cachingHandler)
        {
            _cachingHandler = cachingHandler;
        }

        public void InvalidateResource(Uri resourceToInvalidate)
        {
            _cachingHandler.InvalidateResource(new HttpRequestMessage(HttpMethod.Get, resourceToInvalidate));
        }
    }
}
