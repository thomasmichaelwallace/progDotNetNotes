using System;
using System.IO;
using Grean.AtomEventStore;
using Microsoft.Practices.Unity;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Products_Core.Adapters.Atom;
using Products_Core.Ports.Commands;
using Products_Core.Ports.Events;
using Product_Service;

namespace Products_Core.Ports.Handlers
{
    public class ProductAddedEventHandler : RequestHandler<ProductAddedEvent>
    {
        private readonly IObserver<ProductEntry> _observer;
        private readonly IAmACommandProcessor _commandProcessor;

        //Allows injection of observer for tests
        public ProductAddedEventHandler(IObserver<ProductEntry> observer, IAmACommandProcessor commandProcessor) 
        {
            _observer = observer;
            _commandProcessor = commandProcessor;
        }

        [InjectionConstructor]
        public ProductAddedEventHandler(IAmACommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;

            var storage = new AtomEventsInFiles(new DirectoryInfo(Globals.StoragePath));
            var serializer = new DataContractContentSerializer(
                DataContractContentSerializer
                    .CreateTypeResolver(typeof (ProductEntry).Assembly)
                );

            _observer= new AtomEventObserver<ProductEntry>(
                Globals.ProductEventStreamId,
                25,
                storage,
                serializer
                );
        }

        public override ProductAddedEvent Handle(ProductAddedEvent productAddedEvent)
        {
            _observer.OnNext(new ProductEntry(
                type: ProductEntryType.Created,
                productId: productAddedEvent.ProductId,
                productDescription: productAddedEvent.ProductDescription, 
                productName: productAddedEvent.ProductName, 
                productPrice: productAddedEvent.ProductPrice));

            _commandProcessor.Send(new InvalidateCacheCommand(Globals.ProductFeed));

            return base.Handle(productAddedEvent);
        }
    }
}
