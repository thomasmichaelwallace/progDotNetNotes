using System;
using System.IO;
using Microsoft.Owin.Hosting;
using Product_API.Adapters.Configuration;
using Product_Service;
using Topshelf;

namespace Product_API.Adapters.Service
{
    internal class ProductService : ServiceControl
    {
        private IDisposable _app;
        public bool Start(HostControl hostControl)
        {
            log4net.Config.XmlConfigurator.Configure();
            var configuration = ProductServerConfiguration.GetConfiguration();
            var uri = configuration.Address.Uri;
            Globals.HostName = uri.Host + ":" + uri.Port;
            Globals.StoragePath = Path.Combine(Environment.CurrentDirectory, configuration.Storage.Directory);
            Globals.PageSize = 25;
            Globals.ProductEventStreamId = configuration.Stream.Id;
            Globals.ProductFeed = new Uri("http://" + Globals.HostName + "/feed"); //http://localhost:3416/feed
            _app = WebApp.Start<StartUp>(configuration.Address.Uri.AbsoluteUri);
            return true;
        }


        public bool Stop(HostControl hostControl)
        {
            _app.Dispose();
            return true;
        }

        public void Shutdown(HostControl hostcontrol)
        {
            return;
        }
    }
}
