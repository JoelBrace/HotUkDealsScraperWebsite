using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HotUkDealsScraperWebsite.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetPage(int pageNumber)
        {
            return ScrapePage(pageNumber).GetAwaiter().GetResult();
        }

        private async Task<string> ScrapePage(int pageNumber)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"https://www.hotukdeals.com/tag/electronics?page={pageNumber}").ConfigureAwait(false);

            var html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var document = CreateHtmlDocument(html).DocumentNode;

            var items = document
                .SelectNodes("//article[@class='thread cept-thread-item thread--type-list imgFrame-container--scale thread--deal']")
                .Select(item => item.OuterHtml);

            return string.Join("", items);
        }

        private HtmlDocument CreateHtmlDocument(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            return document;
        }
    }
}