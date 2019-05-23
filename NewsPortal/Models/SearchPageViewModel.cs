using Microsoft.AspNetCore.Mvc;
using NewsPortal.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsPortal.WebSite.Models
{
    public class SearchPageViewModel
    {
        [Display(Name="Mettől:")]
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Meddig:")]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }

        [Display(Name ="Keresés a cikk címében:")]
        public String Title { get; set; }

        [Display(Name = "Keresés a cikk szövegében:")]
        public String Content { get; set; }
        
        public PaginatedList<Article> Result { get; set; }

    }
}
