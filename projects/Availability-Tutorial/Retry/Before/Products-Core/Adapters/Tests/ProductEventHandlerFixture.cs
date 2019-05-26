using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FakeItEasy;
using Grean.AtomEventStore;
using Machine.Specifications;
using paramore.brighter.commandprocessor;
using Products_Core.Adapters.Atom;
using Products_Core.Ports.Commands;
using Products_Core.Ports.Events;
using Products_Core.Ports.Handlers;
using Product_Service;

namespace Products_Core.Adapters.Tests
{
    [Subject(typeof(ProductAddedEventHandler))]
    public class When_raising_a_product_added_event
    {
        private static ProductAddedEventHandler s_handler;
        private static ProductAddedEvent s_event;
        private static IEnumerable<ProductEntry> s_events;
        private static IAmACommandProcessor s_commandProcessor;

        private Establish context = () =>
        {
            var directory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString("N")));

            var eventStreamId = new Guid("79AFBDFA-94D9-474C-BD71-363441FB0443");
            var storage = new AtomEventsInFiles(directory);
            var serializer = new DataContractContentSerializer(
                DataContractContentSerializer
                    .CreateTypeResolver(typeof (ProductEntry).Assembly)
                );
            IObserver<ProductEntry> obs = new AtomEventObserver<ProductEntry>(
                eventStreamId,
                25,
                storage,
                serializer
                );

            s_events = new FifoEvents<ProductEntry>(
                eventStreamId, // a Guid
                storage,       // an IAtomEventStorage object
                serializer);   // an IContentSerializer object

            s_commandProcessor = A.Fake<IAmACommandProcessor>();

            s_handler = new ProductAddedEventHandler(obs, s_commandProcessor);
            s_event = new ProductAddedEvent(123, "Almond Cake", "Nutty cake goodness", 123.45);
        };

        private Because of = () => s_handler.Handle(s_event);

        private It _should_write_an_atom_feed_entry = () => s_events.First().ShouldNotBeNull();

        private It _should_invalidate_the_cache = () => A.CallTo(() => s_commandProcessor.Send(A<InvalidateCacheCommand>.That.Matches(cmd => cmd.ResourceToInvalidate == Globals.ProductFeed)));
    }

    [Subject(typeof(ProductAddedEventHandler))]
    public class When_raising_product_added_events
    {
        private const int PAGE_SIZE = 25;
        private static ProductAddedEventHandler s_handler;
        private static FifoEvents<ProductEntry> s_events;
        private static AtomFeed firstPage;
        private static AtomFeed secondPage;
        private static AtomFeed thirdPage;
        private static AtomFeed fourthPage;
        private static IAmACommandProcessor s_commandProcessor;

        private Establish context = () =>
        {
            var directory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString("N")));

            var eventStreamId = new Guid("79AFBDFA-94D9-474C-BD71-363441FB0443");
            var storage = new AtomEventsInFiles(directory);
            var serializer = new DataContractContentSerializer(
                DataContractContentSerializer
                    .CreateTypeResolver(typeof (ProductEntry).Assembly)
                );
            IObserver<ProductEntry> obs = new AtomEventObserver<ProductEntry>(
                eventStreamId,
                PAGE_SIZE,
                storage,
                serializer
                );

            s_events = new FifoEvents<ProductEntry>(
                eventStreamId, // a Guid
                storage,       // an IAtomEventStorage object
                serializer);   // an IContentSerializer object


            s_commandProcessor = A.Fake<IAmACommandProcessor>();

            s_handler = new ProductAddedEventHandler(obs, s_commandProcessor);
        };

        private Because of = () =>
        {
            foreach (var @event in CreateEventStream(PAGE_SIZE * 3))
            {
                s_handler.Handle(@event);
            }
            firstPage = s_events.ReadFirst();
            secondPage = s_events.ReadNext(firstPage);
            thirdPage = s_events.ReadNext(secondPage);
            fourthPage = s_events.ReadNext(thirdPage);
        };

        private It _should_write_an_atom_feed_entry = () => firstPage.ShouldNotBeNull();
        private It _should_have_a_second_page = () => secondPage.ShouldNotBeNull();
        private It _should_have_a_third_page = () => thirdPage.ShouldNotBeNull();
        private It _should_have_a_fourth_page = () => fourthPage.ShouldBeNull();


        private static IEnumerable<ProductAddedEvent> CreateEventStream(int noOfEntries)
        {
            var productList = new List<ProductAddedEvent>();
            for (var i = 1; i <= noOfEntries; ++i)
            {
                productList.Add(new ProductAddedEvent(123, "Almond Cake", "Nutty cake goodness", 123.45));
            }
            return productList;
        }
    }}
