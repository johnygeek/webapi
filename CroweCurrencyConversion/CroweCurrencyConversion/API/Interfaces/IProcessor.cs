using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CroweCurrencyConversion.API.Interfaces
{
    interface IProcessor
    {
        Task<string> Process(string url, StringDictionary param);
    }
}
