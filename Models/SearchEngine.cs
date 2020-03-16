using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchInvestigation
{
    public class SearchEngine
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }

        public Uri SearchURL { get; set; }

        public string[] SearchResults { get; set; }
    }
}