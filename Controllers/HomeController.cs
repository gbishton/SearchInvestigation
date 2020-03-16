using System;
using System.Collections.Generic;
using System.Linq;
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
            
            SearchEngines.Add(new SearchEngine { Name = "Google", Enabled = false, SearchURL = new Uri("https://www.google.com/search?q=") });
            SearchEngines.Add(new SearchEngine { Name = "Bing", Enabled = false, SearchURL = new Uri("https://www.bing.com/search?q=") });
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

            string search = string.Join("+", model.SearchTerm.Split(' ')) + "num=1";
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
            ViewBag.Message = "Your application description page.";

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

            //googlePage.QuerySelectorAllAsync("cite");

            // Store the HTML of the Google page
            //string googleContent = await googlePage.GetContentAsync();
            //searchLinks = await googlePage.QuerySelectorAllAsync("h3");

            var searchLinks = await page.EvaluateExpressionAsync<string[]>("Array.from(document.querySelectorAll('cite')).map(a => a.innerText);");

  
            searchLinks = searchLinks.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            searchEngine.SearchResults = searchLinks;

            //var searchLinks1 = nodes.map(
            //  
            //     
            //    ").map(function(e) { return e.InnerText})"); ;

            // Pass this to your results view       



            //string googleContent = await googlePage.QuerySelectorAllAsync  .EvaluateExpressionAsync<string>("h3");


        }

    }
}