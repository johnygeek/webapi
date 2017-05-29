using CroweCurrencyConversion.API.Interfaces;
using System;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;
using System.Text;

namespace CroweCurrencyConversion.API.Processor
{
    public class YahooProcessor : IProcessor
    {
        public async Task<string> Process(string url, StringDictionary requestParams)
        {
            WebClient client = new WebClient();
            var responseTask = client.DownloadStringTaskAsync(GetRequestedUri(url, requestParams));
            return await ConvertCurrency(Convert.ToDouble(requestParams[Constants.AMOUNT]), responseTask);
        }

        private async Task<string> ConvertCurrency(double amount, Task<string> task)
        {
            string result = string.Empty;
            double convFactor;
            var resResult = await task;
            if (resResult.IndexOf(",") > -1)
            {
                var arResult = resResult.Split(new char[] { ',' });
                result = Double.TryParse(arResult[1], out convFactor) ? Math.Round(amount * convFactor,2).ToString() : string.Empty;
            }
            return result;
        }

        private string GetRequestedUri(string url, StringDictionary requestParams)
        {
            StringBuilder uriBuilder = new StringBuilder(url);
            uriBuilder.Append("?e=.csv&f=sl1d1t1&s=" + requestParams[Constants.FROMCUR].ToUpper() + requestParams[Constants.TOCUR].ToUpper() + "=X");
            return uriBuilder.ToString();
        }
    }
}