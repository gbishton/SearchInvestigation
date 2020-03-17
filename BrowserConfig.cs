using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PuppeteerSharp;


namespace SearchInvestigation
{
    public class BrowserConfig
    {
        public static async Task DownloadChromeRevision()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
        }
    }
}
