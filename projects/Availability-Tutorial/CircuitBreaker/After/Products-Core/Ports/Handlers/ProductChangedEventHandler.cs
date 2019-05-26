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
    public class ProductChangedEventHandler : RequestHandler<ProductChangedEvent>
    {
        private readonly IObserver<ProductEntry> _observer;
        private readonly IAmACommandProcessor _commandProcessor;

        public ProductChangedEventHandler(IObserver<ProductEntry> observer, IAmACommandProcessor commandProcessor) 
        {
            _observer = observer;
            _commandProcessor = commandProcessor;
        }

        [InjectionConstructor]
        public ProductChangedEventHandler(IAmACommandProcessor commandProcessor)
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

        public override ProductChangedEvent Handle(ProductChangedEvent productChangedEvent)
        {
            _observer.OnNext(new ProductEntry(
                type: ProductEntryType.Updated,
                productId: productChangedEvent.ProductId,
                productDescription: productChangedEvent.ProductDescription, 
                productName: productChangedEvent.ProductName, 
                productPrice: productChangedEvent.ProductPrice));

            _commandProcessor.Send(new InvalidateCacheCommand(Globals.ProductFeed));

            return base.Handle(productChangedEvent);
        }
    }
}
