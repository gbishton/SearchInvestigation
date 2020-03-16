using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SearchInvestigation
{
    public class SearchInvestigationViewModel
    {
        public List<SearchEngine> SearchEngines { get; set; }

        [Required(ErrorMessage = "Please enter a Search Term")]
        public string SearchTerm { get; set; }
    }
}