using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsPortal.Data;

namespace NewsPortal.Admin.Model
{
    public interface INewsPortalModel
    {

        List<ArticleListElement> ArticleList { get; }

        ArticleDTO ArticleToEdit { get; set; }

        Boolean IsUserLoggedIn { get; }      

        event EventHandler<ArticleListEventArgs> ArticleChanged;
        event EventHandler<ArticleListEventArgs> ArticleCreated;
        event EventHandler<PictureEventArgs> PictureCreated;

        Task LoadArticleAsync(int articleId);


        void CreatePicture(Int32 articleId, Byte[] imageSmall, Byte[] imageLarge);

        Task UpdateArticle(ArticleDTO article);

        void DeleteArticle(ArticleListElement article);

        void DeletePicture(PictureDTO picture);

        Task LoadAsync();

        Task CreateArticleAsync(ArticleDTO articleToSave);

        Task<Boolean> LoginAsync(String userName, String userPassword);

        Task<Boolean> LogoutAsync();
    }
        
}
