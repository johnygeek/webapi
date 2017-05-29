using CroweCurrencyConversion.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace CroweCurrencyConversion.API.Processor
{
    public class SecureAPIProcessor : IProcessor
    {
        public async Task<string> Process(string url, StringDictionary param)
        {
            var token = GetAPIToken(param[Constants.USERNAME], param[Constants.PASSWORD], param[Constants.LOGINURL]).Result;
            var response = await GetRequest(token, param[Constants.LOGINURL], url);
            return response;
        }

        private async Task<string> GetAPIToken(string userName, string password, string apiBaseUri)
        {
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.APPLICATIONJSONMIMETYPE));

                //setup login data
                var formContent = new FormUrlEncodedContent(new[]
                {
                 new KeyValuePair<string, string>(Constants.GRANT_TYPE, Constants.PASSWORD),
                 new KeyValuePair<string, string>(Constants.USERNAME, userName),
                 new KeyValuePair<string, string>(Constants.PASSWORD, password),
                });

                //send request
                HttpResponseMessage responseMessage = await client.PostAsync("/Token", formContent);

                //get access token from response body
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseJson);
                return jObject.GetValue(Constants.ACCESS_TOKEN).ToString();
            }
        }

        private async Task<string> GetRequest(string token, string apiBaseUri, string requestPath)
        {
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add(Constants.AUTHORIZATION, Constants.BEARER + token);

                //make request
                HttpResponseMessage response = await client.GetAsync(requestPath);
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
        }
    }
}