using System;
using System.Configuration;

namespace Product_API.Adapters.Configuration
{
    public class ProductServerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("address")]
        public ProductUriSpecification Address
        {
            get { return this["address"] as ProductUriSpecification; }
            set { this["address"] = value; }
        }

        [ConfigurationProperty("storage")]
        public AtomEventStorageDirectory Storage
        {
            get { return this["storage"] as AtomEventStorageDirectory ; }
            set { this["storage"] = value; }
        }

        [ConfigurationProperty("eventstream")]
        public EventStream Stream
        {
            get { return this["eventstream"] as EventStream; }
            set { this["eventstream"] = value; }
        } 

        public static ProductServerConfiguration GetConfiguration()
        {
            var configuration =
                ConfigurationManager.GetSection("productServer") as ProductServerConfiguration;

            if (configuration != null)
                return configuration;

            return new ProductServerConfiguration();
        }
    }

    public class ProductUriSpecification : ConfigurationElement
    {
        [ConfigurationProperty("uri", DefaultValue = "http://localhost:3416/", IsRequired = true)]
        public Uri Uri
        {
            get { return (Uri)this["uri"]; }
            set { this["uri"] = value; }
        }
    }

    public class AtomEventStorageDirectory : ConfigurationElement
    {
        [ConfigurationProperty("directory", DefaultValue = ".atom_storage", IsRequired = false)]
        public string Directory
        {
            get { return (string)this["directory"]; }
            set { this["directory"] = value; }
        }
    }

    public class EventStream : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value.ToString(); }
        }

        [ConfigurationProperty("id", IsRequired = true)]
        public Guid Id
        {
            get { return (Guid)this["id"]; }
            set { this["id"] = value; }
        }
    }
}
