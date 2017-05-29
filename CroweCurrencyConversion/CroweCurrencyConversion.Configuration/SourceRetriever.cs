using System.Configuration;

namespace CroweCurrencyConversion.Configuration
{
    public class SourceRetriever
    {
        private static readonly SourceRetrieverSection _config;

        static SourceRetriever()
        {
            _config = (ConfigurationManager.GetSection("sourceRetriever") as SourceRetrieverSection);
        }

        public static SourceElementCollection Sources
        {
            get
            {
                return _config.Sources;
            }
        }
    }
}