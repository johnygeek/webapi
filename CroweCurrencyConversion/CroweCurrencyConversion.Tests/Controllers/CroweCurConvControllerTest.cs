using System;
using CroweCurrencyConversion.API;
using CroweCurrencyConversion.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace CroweCurrencyConversion.Tests.Controllers
{
    [TestClass]
    public class CroweCurConvControllerTest
    {

        [TestInitialize]
        public void LoadSources()
        {
            WebApiConfig.Sources = SourceRetriever.Sources;
        }

        [TestCleanup]
        public void CleaupSources()
        {
            WebApiConfig.Sources = null;
        }

        [TestMethod]
        public async Task CurrencyConvertYahooWithValidParams()
        {
            var controller = new CroweCurConvController();
            decimal convertedVal;
            var response = await controller.Get("yahoo", "USD", "GBP", 1200.13m, "28052017");
            Assert.IsTrue(Decimal.TryParse((response as OkNegotiatedContentResult<string>).Content, out convertedVal));
            Assert.IsTrue(convertedVal < 1200.13m);
        }

        [TestMethod]
        public async Task CurrencyConvertYahooWithInvalidParams()
        {
            var controller = new CroweCurConvController();
            decimal convertedVal;
            var response = await controller.Get("yahoo", "USD", "a", 1200.13m, "28052017");
            Assert.IsFalse(Decimal.TryParse((response as OkNegotiatedContentResult<string>).Content, out convertedVal));
        }

        [TestMethod]
        public async Task CurrencyConvertWithDisabledSource()
        {
            var controller = new CroweCurConvController();
            decimal convertedVal;
            var response = await controller.Get("xe", "USD", "GBP", 1200.13m, "28052017");
            Assert.IsFalse(Decimal.TryParse((response as OkNegotiatedContentResult<string>).Content, out convertedVal));
            Assert.IsTrue((response as OkNegotiatedContentResult<string>).Content.EndsWith("disabled"));
        }
    }
}
