using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsPortal.Persistence;

namespace NewsPortal.WebSite.Models
{
    public class HomePageViewModel
    {
        public Article LeadingArticle { get; set; }
        public IList<Article> Articles { get; set; }
    }
}
