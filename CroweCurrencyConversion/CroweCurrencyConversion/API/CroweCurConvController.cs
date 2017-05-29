using CroweCurrencyConversion.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace CroweCurrencyConversion.API
{
    public class CroweCurConvController : ApiController
    {

        private SourceElementCollection _sources;

        public CroweCurConvController()
        {
            _sources = WebApiConfig.Sources;
        }

        // GET: api/CroweCurConv?source=?&fromCur=?&toCur=?&date=?
        public async Task<IHttpActionResult> Get(string source, string fromCur, string toCur, decimal amount, string date)
        {
            SourceElement processingSource = null;
            StringDictionary dictParam = null;
            foreach (SourceElement sourceElem in _sources)
            {
                if (sourceElem.Name == source)
                {
                    processingSource = sourceElem;
                    break;
                }
            }

            if (processingSource != null)
            {
                if (processingSource.Enabled)
                {
                    var processor = ProcessorFactory.CreateandReturnObj(processingSource.Name);

                    if (!processingSource.Secure)
                        dictParam = GetRequestParamDictionary(fromCur, toCur, amount.ToString(), date);
                    else
                        dictParam = GetRequestParamDictionary(fromCur, toCur, amount.ToString(), date, processingSource.LoginUrl);
                    try
                    {
                        var responseString = await processor.Process(processingSource.Url, dictParam);
                        return Ok(responseString);
                    }
                    catch (WebException ex)
                    {
                        using (WebResponse response = ex.Response)
                        {
                            HttpWebResponse httpWebResponse = response as HttpWebResponse;
                            if (httpWebResponse.StatusCode == HttpStatusCode.BadRequest)
                                return BadRequest();
                            else if (httpWebResponse.StatusCode == HttpStatusCode.InternalServerError)
                                return InternalServerError();
                            else if (httpWebResponse.StatusCode == HttpStatusCode.NotFound)
                                return NotFound();
                        }
                    }
                }
                else
                {
                    return Ok(Constants.SOURCEDISABLEDMSG);
                } 
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        private StringDictionary GetRequestParamDictionary(string fromCur, string toCur, string amount, string date, string loginUrl = null)
        {
            StringDictionary dictParam = new StringDictionary();
            if (!string.IsNullOrEmpty(fromCur))
                dictParam.Add(Constants.FROMCUR, fromCur);
            if (!string.IsNullOrEmpty(toCur))
                dictParam.Add(Constants.TOCUR, toCur);
            if (!string.IsNullOrEmpty(amount))
                dictParam.Add(Constants.AMOUNT, amount);
            if (!string.IsNullOrEmpty(loginUrl))
            {
                dictParam.Add(Constants.LOGINURL, loginUrl);
                dictParam.Add(Constants.USERNAME, this.Request.Headers.GetValues(Constants.USERNAME).FirstOrDefault());
                dictParam.Add(Constants.PASSWORD, this.Request.Headers.GetValues(Constants.PASSWORD).FirstOrDefault());
            }
            return dictParam;
        }
    }
}