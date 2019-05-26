using System;
using System.Threading.Tasks;
using System.Web.Configuration;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Polly;
using Store_Core.Adapters.Atom;
using Store_Core.Adapters.DataAccess;
using Store_Core.Ports.Commands;

namespace Store_Core.Adapters.Service
{
    public class Consumer
    {
        private readonly ILastReadFeedItemDAO _lastReadFeedItemDao;
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly ILog _logger;
        private Task _controlTask;
        private bool _consumeFeed;
        private readonly AtomFeedGateway _atomFeedGateway;
        private readonly Policy _retryPolicy;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Consumer(ILastReadFeedItemDAO lastReadFeedItemDao, IAmACommandProcessor commandProcessor, ILog logger)
        {
            _lastReadFeedItemDao = lastReadFeedItemDao;
            _commandProcessor = commandProcessor;
            _logger = logger;
            _atomFeedGateway = new AtomFeedGateway(_lastReadFeedItemDao, _logger);
            _retryPolicy = Policy
                .Handle<ApplicationException>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromMilliseconds(10),
                    TimeSpan.FromMilliseconds(20),
                    TimeSpan.FromMilliseconds(30),   
                }, (exception, timespan) =>
                {
                    logger.WarnException("Error connecting to the server - will retry in {0} milliseconds", exception, timespan.Milliseconds);
                });
        }

        public void Consume(Uri uri)
        {
            _consumeFeed = true;
            _controlTask = Task.Factory.StartNew(
                () =>
                {
                    _logger.DebugFormat("Running Consumer loop: reading reference data.");
                    while (_consumeFeed)
                    {
                        try
                        {
                            _retryPolicy.Execute(() =>
                            {
                                foreach (var entry in _atomFeedGateway.GetFeedEntries(uri))
                                {
                                    _logger.Debug("Writing Reference Data change");
                                    switch (entry.Type)
                                    {
                                        case ProductEntryType.Created:
                                            _commandProcessor.Send(new AddProductCommand(entry.ProductName,
                                                entry.ProductDescription,
                                                entry.Price));
                                            break;
                                        case ProductEntryType.Updated:
                                            _commandProcessor.Send(new ChangeProductCommand(entry.ProductId,
                                                entry.ProductName,
                                                entry.ProductDescription, entry.Price));
                                            break;
                                        case ProductEntryType.Deleted:
                                            _commandProcessor.Send(new RemoveProductCommand(entry.ProductId));
                                            break;
                                    }
                                }
                                Task.Delay(Globals.PollingIntervalInMilliseconds).Wait();
                            });
                        }
                        catch (ApplicationException)
                        {
                            _logger.DebugFormat("Exiting retry loop to check for cancellation, will restart loop in {0} milliseconds if not cancelled", Globals.ErrorDelayInMilliseconds);
                             Task.Delay(Globals.ErrorDelayInMilliseconds).Wait();
                        }


                    }
                },
                TaskCreationOptions.LongRunning
            );
        }

        public Task End()
        {
            _consumeFeed = false;

            return _controlTask;
        }
    }
}
