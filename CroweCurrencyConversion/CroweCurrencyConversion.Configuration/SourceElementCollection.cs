using System.Configuration;
namespace CroweCurrencyConversion.Configuration
{
    [ConfigurationCollection(typeof(SourceElement))]
    public class SourceElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SourceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SourceElement)element).Name;
        }
    }
}