using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsPortal.Data;

namespace NewsPortal.Admin.Model
{
    public interface INewsPortalModel
    {
        IReadOnlyList<ArticleDTO> Articles { get; }

        List<ArticleListElement> ArticleList { get; }

        Boolean IsUserLoggedIn { get; }

        event EventHandler<ArticleEventArgs> ArticleChanged;

        void CreateArticle(ArticleDTO article);

        void CreatePicture(Int32 articleId, Byte[] imageSmall, Byte[] imageLarge);

        void UpdateArticle(ArticleDTO article);

        void DeleteArticle(ArticleDTO article);

        void DeletePicture(PictureDTO picture);

        Task LoadAsync();

        Task SaveAsync();

        Task<Boolean> LoginAsync(String userName, String userPassword);

        Task<Boolean> LogoutAsync();
    }
        
}
