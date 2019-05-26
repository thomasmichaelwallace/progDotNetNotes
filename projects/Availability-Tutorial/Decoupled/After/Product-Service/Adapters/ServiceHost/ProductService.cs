using System;
using Microsoft.Practices.Unity;
using Orders_Service.Adapters.ServiceHost;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.messaginggateway.rmq;
using paramore.brighter.serviceactivator;
using Polly;
using Products_Core.Adapters.DataAccess;
using Products_Core.Ports.Commands;
using Products_Core.Ports.Handlers;
using Product_Service.Ports.Mappers;
using Topshelf;

namespace Product_Service.Adapters.ServiceHost
{
    class ProductService: ServiceControl
    {
        private Dispatcher _dispatcher;
        public ProductService()
        {
            log4net.Config.XmlConfigurator.Configure();

            var logger = LogProvider.For<ProductService>();

            var container = new UnityContainer();
            container.RegisterInstance(typeof(ILog), LogProvider.For<ProductService>(), new ContainerControlledLifetimeManager());
            container.RegisterType<AddProductCommandMessageMapper>();
            container.RegisterType<AddProductCommandHandler>();
            container.RegisterType<IProductsDAO, ProductsDAO>();

            var handlerFactory = new UnityHandlerFactory(container);
            var messageMapperFactory = new UnityMessageMapperFactory(container);

            var subscriberRegistry = new SubscriberRegistry();
            subscriberRegistry.Register<AddProductCommand, AddProductCommandHandler>();

            //create policies
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(new[]
                    {
                        TimeSpan.FromMilliseconds(50),
                        TimeSpan.FromMilliseconds(100),
                        TimeSpan.FromMilliseconds(150)
                    });

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(1, TimeSpan.FromMilliseconds(500));

            var policyRegistry = new PolicyRegistry()
            {
                {CommandProcessor.RETRYPOLICY, retryPolicy},
                {CommandProcessor.CIRCUITBREAKER, circuitBreakerPolicy}
            };

            var commandProcessor = CommandProcessorBuilder.With()
                .Handlers(new HandlerConfiguration(subscriberRegistry, handlerFactory))
                .Policies(policyRegistry)
                .NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory())
                .Build();

            container.RegisterInstance(typeof (IAmACommandProcessor), commandProcessor);

            //create message mappers
            var messageMapperRegistry = new MessageMapperRegistry(messageMapperFactory);
            messageMapperRegistry.Register<AddProductCommand, AddProductCommandMessageMapper>();

            _dispatcher = DispatchBuilder.With()
                .CommandProcessor(commandProcessor)
                .MessageMappers(messageMapperRegistry)
                .ChannelFactory(new InputChannelFactory(new RmqMessageConsumerFactory(), new RmqMessageProducerFactory()))
                .ConnectionsFromConfiguration()
                .Build();
        }

        public bool Start(HostControl hostControl)
        {
            _dispatcher.Receive();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _dispatcher.End().Wait();
            _dispatcher = null;
            return true;
        }

        public void Shutdown(HostControl hostcontrol)
        {
            if (_dispatcher != null)
                _dispatcher.End().Wait();
        }
    }
}
