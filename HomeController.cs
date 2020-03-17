using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PuppeteerSharp;

namespace SearchInvestigation
{
    public class HomeController : Controller
    {
        List<SearchEngine> SearchEngines = new List<SearchEngine>();
        List<string> SearchResults = new List<string>();
        public HomeController()
        {
            
            SearchEngines.Add(new SearchEngine { Name = "Google", Enabled = false, SearchURL = new Uri("https://www.google.com/search?q="), Selector= @"Array.from(document.querySelectorAll('div.r a')).map(a => a.href);" });
            SearchEngines.Add(new SearchEngine { Name = "Bing", Enabled = false, SearchURL = new Uri("https://www.bing.com/search?q="), Selector= @"Array.from(document.querySelectorAll('li.b_algo a')).map(a => a.href);" });
        }
        
        public ActionResult Index()
        {
            var viewModel = new SearchInvestigationViewModel();
            viewModel.SearchEngines = this.SearchEngines;
            return View(viewModel);
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(SearchInvestigationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            string search = string.Join("+", model.SearchTerm.Split(' '));
            var searchResults = new List<string>();

            // Create an instance of the browser and configure launch options
            Browser browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            foreach (var searchEngine in model.SearchEngines)
            {
                if (searchEngine.Enabled == true)
                {
                    await RunSearch(searchEngine, search,  browser);
                }
            }

            // Close the browser
            await browser.CloseAsync();

            return View("SearchResults", model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Search investigation application.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        async Task RunSearch(SearchEngine searchEngine, string searchQuery, Browser browser)
        {
            // Create a new page
            Page page = await browser.NewPageAsync();
            await page.GoToAsync(searchEngine.SearchURL + searchQuery);
           
            var searchLinks = await page.EvaluateExpressionAsync<string[]>(searchEngine.Selector);
            searchLinks = searchLinks.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();

            //Create url
            List<Uri> searchUrls = new List<Uri>();
            foreach (var link in searchLinks)
            {
                Uri uriResult;
                if(Uri.TryCreate(link, UriKind.Absolute, out uriResult))
                {
                    searchUrls.Add(uriResult);
                }
            }
            searchEngine.SearchResults = searchUrls;
        }
    }
}