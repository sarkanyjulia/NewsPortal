using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Persistence;
using NewsPortal.WebSite.Models;

namespace NewsPortal.WebSite.Controllers
{
    public class ArchivesController : Controller
    {
        private INewsPortalService _service;

        public ArchivesController(INewsPortalService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index(int? pageNumber)
        {          
            int pageSize = 20;          
            return View("Index", PaginatedList<Article>.Create(_service.GetOrderedArticles(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Search()
        {
            SearchPageViewModel viewModel = new SearchPageViewModel();
            return View("Search", viewModel);
        }

        [HttpGet]
        public IActionResult Result(DateTime? dateFrom, DateTime? dateTo, String title, String content, int? pageNumber)
        {

            int pageSize = 20;
            SearchPageViewModel viewModel = new SearchPageViewModel();
            if (ModelState.IsValid)
            {
                viewModel.DateFrom = dateFrom;
                viewModel.DateTo = dateTo;
                viewModel.Title = title;
                viewModel.Content = content;
                viewModel.Result = PaginatedList<Article>.Create(_service.FindArticles(dateFrom, dateTo, title, content), pageNumber ?? 1, pageSize);
            }
            return View("Search", viewModel);
        }      
    }
}