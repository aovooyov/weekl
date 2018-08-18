using System.Configuration;

namespace Weekl.Api.Infrastructure.Configuration
{
    public class CorsSection : ConfigurationSection
    {
        [ConfigurationProperty("cors")]
        public CorsCollection Cors => this["cors"] as CorsCollection;
    }

    public class CorsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CorsType();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CorsType)element).Origin;
        }
    }

    public class CorsType : ConfigurationElement
    {
        [ConfigurationProperty("origin", IsRequired = true, IsKey = true)]
        public string Origin => this["origin"] as string;

        [ConfigurationProperty("headers", IsRequired = false)]
        public string Headers => this["headers"] as string;

        [ConfigurationProperty("methods", IsRequired = false)]
        public string Methods => this["methods"] as string;
    }
}