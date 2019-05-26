using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Grean.AtomEventStore;
using paramore.brighter.commandprocessor.Logging;
using Store_Core.Adapters.DataAccess;

namespace Store_Core.Adapters.Atom
{
    public class AtomFeedGateway
    {
        private ThreadLocal<HttpClient> _client;
        private readonly ILastReadFeedItemDAO _lastReadFeedItemDao;
        public readonly ILog _logger;

        public AtomFeedGateway(ILastReadFeedItemDAO lastReadFeedItemDao, ILog logger)
        {
            _lastReadFeedItemDao = lastReadFeedItemDao;
            _logger = logger;
        }


        public IEnumerable<ProductEntry> GetFeedEntries(Uri uri)
        {
            try
            {
                _logger.DebugFormat("Reading reference data from {0}", uri);
                var response = Client().GetAsync(uri).Result;
                response.EnsureSuccessStatusCode();

                var serializer = new DataContractContentSerializer(DataContractContentSerializer.CreateTypeResolver(typeof(ProductEntry).Assembly));
                var feed = AtomFeed.Parse(response.Content.ReadAsStringAsync().Result, serializer);
                var reader = new ReferenceDataFeedReader<ProductEntry>(_lastReadFeedItemDao, feed);
                return reader;
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.Flatten().InnerExceptions)
                {
                    _logger.InfoFormat("Threw exception getting feed from the Server {0}", uri, exception.Message);
                }

                throw new ApplicationException(string.Format("Error retrieving the feed from the server, see log for details"));
            }
        }


        public HttpClient Client()
        {
            _client = new ThreadLocal<HttpClient>(() => CreateClient());
            return _client.Value;
        }



        private HttpClient CreateClient()
        {
            var requestHandler = new WebRequestHandler
            {
                AllowPipelining = true,
                AllowAutoRedirect = true,
                CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate)
            };
            var client = HttpClientFactory.Create(requestHandler);
            client.Timeout = TimeSpan.FromMilliseconds(200);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            return client;
        }
    }
}
