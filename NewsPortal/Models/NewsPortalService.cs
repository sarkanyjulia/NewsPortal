using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsPortal.Persistence;

namespace NewsPortal.WebSite.Models
{
    public class NewsPortalService : INewsPortalService
    {
        private readonly NewsPortalContext _context;

        public NewsPortalService(NewsPortalContext context)
        {
            _context = context;
            
        }
        public IEnumerable<Article> Articles => _context.Articles;

        public IQueryable<Article> GetOrderedArticles()
        {
            return _context.Articles.AsNoTracking().OrderByDescending(a => a.LastModified);
        }
        public Article GetArticle(int? id)
        {
            return _context.Articles
                .Include(article => article.Author)
                .Include(article => article.Pictures)
                .FirstOrDefault(article => article.Id == id);
        }

        public IEnumerable<Article> GetFreshArticles()
        {
            return _context.Articles
                .Where(article => article.Lead == false)
                .OrderByDescending(article => article.LastModified)
                .Take(10);
        }

        public Article GetLeadingArticle()
        {
            return _context.Articles
                .Include(article => article.Pictures)
                .FirstOrDefault(article => article.Lead==true);
        }

        public Byte[] GetMainImage(Int32? articleId)
        {
            if (articleId == null)
                return null;

            return _context.Pictures
                .Where(image => image.ArticleId == articleId)
                .Select(image => image.ImageSmall)
                .FirstOrDefault();
        }

        public byte[] GetLargePictureById(int? pictureId)
        {
            if (pictureId == null)
                return null;

            return _context.Pictures
                .Where(picture => picture.Id == pictureId)
                .Select(picture => picture.ImageLarge)
                .FirstOrDefault();
        }

        public byte[] GetSmallPictureById(int? pictureId)
        {
            if (pictureId == null)
                return null;

            return _context.Pictures
                .Where(picture => picture.Id == pictureId)
                .Select(picture => picture.ImageSmall)
                .FirstOrDefault();
        }

        public IEnumerable<int> GetPictureIds(int? articleId)
        {
            if (articleId == null)
                return null;
            return _context.Pictures
                .Where(image => image.ArticleId == articleId)
                .Select(image => image.Id);
        }


        public IQueryable<Article> FindArticles(DateTime? dateFrom, DateTime? dateTo, String title, String content)
        {
            IQueryable<Article> result = _context.Articles;
            if (dateFrom != null)
            {
                result = result.Where(article => (article.LastModified.Date >= dateFrom));
            }
            if (dateTo !=null)
            {
                result = result.Where(article => (article.LastModified.Date <= dateTo));
            }
            if (!String.IsNullOrEmpty(title))
            {
                result = result.Where(article => article.Title.ToLower().Contains(title.ToLower()));
            }
            if (!String.IsNullOrEmpty(content))
            {
                result = result.Where(article => article.Content.ToLower().Contains(content.ToLower()));
            }
            return result.OrderByDescending(a => a.LastModified);
        }
    }
}
