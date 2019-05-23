using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.WebSite.Models;
using NewsPortal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace NewsPortal.WebSite.Controllers
{
    public class HomeController : Controller       
    {
        private INewsPortalService _service;

        public HomeController(INewsPortalService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            HomePageViewModel viewModel = new HomePageViewModel();
            viewModel.LeadingArticle = _service.GetLeadingArticle();
            viewModel.Articles = _service.GetFreshArticles().ToList();
            return View("Index", viewModel);
        }

        public IActionResult Article(int? articleId)
        {
            Article article = _service.GetArticle(articleId);
            return View("Article", article);
        }

        public FileResult MainPictureForArticle(Int32? articleId)
        {          
            Byte[] imageContent = _service.GetMainImage(articleId);

            if (imageContent == null) 
                return null;

            return File(imageContent, "image/png");
        }

        public IActionResult Gallery(int articleId)
        {
            GalleryViewModel viewModel = new GalleryViewModel();
            viewModel.ArticleId = articleId;
            viewModel.PictureIds = _service.GetPictureIds(articleId).ToList();
            return View("Gallery", viewModel);
        }

        public FileResult LargePicture(Int32? pictureId)
        {           
            Byte[] imageContent = _service.GetLargePictureById(pictureId);

            if (imageContent == null)
                return null;

            return File(imageContent, "image/png");
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
