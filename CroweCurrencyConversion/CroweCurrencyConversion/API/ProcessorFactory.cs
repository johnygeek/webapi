using CroweCurrencyConversion.API.Interfaces;
using CroweCurrencyConversion.API.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CroweCurrencyConversion.API
{
    class ProcessorFactory
    {
        public static IProcessor CreateandReturnObj(string processorName)
        {
            IProcessor processor = null;
            if (processorName.ToLower() == "yahoo")
            {
                processor = new YahooProcessor();
            }
            //create other source API processors here 
            return processor;
        }
    }
}