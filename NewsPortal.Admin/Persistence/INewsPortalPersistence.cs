using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Data;

namespace NewsPortal.Admin.Persistence
{
    public interface INewsPortalPersistence
    {
        Task<IEnumerable<ArticleListElement>> ReadArticlesAsync();
        Task<Boolean> CreateArticleAsync(ArticleDTO article);
        Task<Boolean> UpdateArticleAsync(ArticleDTO article);
        Task<Boolean> DeleteArticleAsync(ArticleDTO article);
        Task<Boolean> CreatePictureAsync(PictureDTO image);
        Task<Boolean> DeletePictureAsync(PictureDTO image);       
        Task<Boolean> LoginAsync(String userName, String userPassword);
        Task<Boolean> LogoutAsync();
    }
}
