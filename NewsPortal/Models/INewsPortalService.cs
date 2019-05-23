using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Persistence;

namespace NewsPortal.WebSite.Models
{
    public interface INewsPortalService
    {
        IEnumerable<Article> Articles { get; }

        IQueryable<Article> GetOrderedArticles();

        IEnumerable<Article> GetFreshArticles();
    

        Article GetLeadingArticle();

        Article GetArticle(int? id);

        Byte[] GetMainImage(Int32? articleId);

        IEnumerable<int> GetPictureIds(int? articleId);

        IQueryable<Article> FindArticles(DateTime? dateFrom, DateTime? dateTo, String title, String content);

        byte[] GetLargePictureById(int? pictureId);
        
        byte[] GetSmallPictureById(int? pictureId);
        
    }
}
