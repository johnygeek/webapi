using System.Configuration;

namespace CroweCurrencyConversion.Configuration
{
    public class SourceRetrieverSection: ConfigurationSection
    {
        [ConfigurationProperty("sources", IsDefaultCollection = true)]
        public SourceElementCollection Sources
        {
            get { return (SourceElementCollection)this["sources"]; }
            set { this["sources"] = value; }
        }
    }
}