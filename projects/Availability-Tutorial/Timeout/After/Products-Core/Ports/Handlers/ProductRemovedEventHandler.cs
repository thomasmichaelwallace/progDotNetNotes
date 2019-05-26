using System;
using System.IO;
using Grean.AtomEventStore;
using Microsoft.Practices.Unity;
using paramore.brighter.commandprocessor;
using Products_Core.Adapters.Atom;
using Products_Core.Ports.Events;
using Product_Service;

namespace Products_Core.Ports.Handlers
{
    public class ProductRemovedEventHandler : RequestHandler<ProductRemovedEvent>
    {
        private readonly IObserver<ProductEntry> _observer;
        private readonly IAmACommandProcessor _commandProcessor;

        //constuctor intended for tests
        public ProductRemovedEventHandler(IObserver<ProductEntry> observer, IAmACommandProcessor commandProcessor) 
        {
            _observer = observer;
            _commandProcessor = commandProcessor;
        }

        [InjectionConstructor]
        public ProductRemovedEventHandler(IAmACommandProcessor commandProcessor)
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

        public override ProductRemovedEvent Handle(ProductRemovedEvent productRemovedEvent)
        {
            _observer.OnNext(new ProductEntry(
                type: ProductEntryType.Deleted,
                productId: productRemovedEvent.ProductId,
                productDescription: productRemovedEvent.ProductDescription, 
                productName: productRemovedEvent.ProductName, 
                productPrice: productRemovedEvent.ProductPrice));

            return base.Handle(productRemovedEvent);
        }
    }
}
