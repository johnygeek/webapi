using System.Configuration;

namespace CroweCurrencyConversion.Configuration
{
    public class SourceElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("url", IsRequired = true, DefaultValue = "http://localhost")]
        //[RegexStringValidator(@"http?\://\S+")]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }

        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("secure", IsRequired = false, DefaultValue = false)]
        public bool Secure
        {
            get { return (bool)this["secure"]; }
            set { this["secure"] = value; }
        }

        [ConfigurationProperty("loginUrl", IsRequired = false, DefaultValue = "http://localhost")]
        public string LoginUrl
        {
            get { return (string)this["loginUrl"]; }
            set { this["loginUrl"] = value; }
        }
    }
}