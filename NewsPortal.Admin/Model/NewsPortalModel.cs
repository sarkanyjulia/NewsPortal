using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Data;
using NewsPortal.Admin.Persistence;

namespace NewsPortal.Admin.Model
{
    public class NewsPortalModel : INewsPortalModel
    {       
        private int _generatedId = 0;

        private INewsPortalPersistence _persistence;
        private List<ArticleListElement> _articleList;

        public UserDTO RecentUser { get; private set; }

        public NewsPortalModel(INewsPortalPersistence persistence)
        {
            if (persistence == null)
                throw new ArgumentNullException(nameof(persistence));

            IsUserLoggedIn = false;
            _persistence = persistence;
        }

        public ArticleDTO ArticleToEdit { get; set; }

        public List<ArticleListElement> ArticleList
        {
            get { return _articleList; }
        }
        public bool IsUserLoggedIn { get; private set; }
        

        public event EventHandler<ArticleListEventArgs> ArticleChanged;
        public event EventHandler<ArticleListEventArgs> ArticleCreated;
        public event EventHandler<PictureEventArgs> PictureCreated;


        public void CreatePicture(int articleId, byte[] imageSmall, byte[] imageLarge)
        {          
            PictureDTO picture = new PictureDTO
            {
                Id = GetNextId(),
                ArticleId = articleId,
                ImageSmall = imageSmall,
                ImageLarge = imageLarge
            };
            OnPictureCreated(picture);
        }

        public void DeleteArticle(ArticleListElement article)
        {
            _persistence.DeleteArticleAsync(article.Id);
            _articleList.Remove(article);
        }
       

        public async Task LoadAsync()
        {          
            _articleList = (await _persistence.ReadArticlesAsync()).ToList();
        }

        public async Task LoadArticleAsync(int articleId)
        {
            ArticleToEdit = await _persistence.ReadArticleAsync(articleId);
        }

        public async Task<bool> LoginAsync(string userName, string userPassword)
        {
            IsUserLoggedIn = await _persistence.LoginAsync(userName, userPassword); 
            if (IsUserLoggedIn)
            {
                RecentUser = await _persistence.GetUser();
            }
            return IsUserLoggedIn;
        }

        public async Task<bool> LogoutAsync()
        {
            if (!IsUserLoggedIn)
                return true;

            IsUserLoggedIn = !(await _persistence.LogoutAsync());
            RecentUser = null;
            return IsUserLoggedIn;
        }

        public async Task CreateArticleAsync(ArticleDTO article)
        {
            if (IsFormInvalid(article))
            {
                throw new InvalidFormException();
            }
            article.UserId = RecentUser.Id;
            await _persistence.CreateArticleAsync(article);
            ArticleListElement newArticleElementList = new ArticleListElement
            {
                Id = article.Id,
                Title = article.Title,
                LastModified = article.LastModified,
                AuthorName = RecentUser.Name
            };
            _articleList.Add(newArticleElementList);
            OnArticleCreated(newArticleElementList);
        }

        public async Task UpdateArticle(ArticleDTO article)
        {
            if ( IsFormInvalid(article) )
            {
                throw new InvalidFormException();
            }

            ArticleDTO articleToSave = new ArticleDTO {
                Id = article.Id,
                Title = article.Title,
                Summary = article.Summary,
                Content = article.Content,
                UserId = article.UserId,
                LastModified = article.LastModified,
                Lead = article.Lead
            };
            await _persistence.UpdateArticleAsync(articleToSave);

            foreach (PictureDTO picture in article.Pictures)
            {
                if ( picture.Id<0 )
                {
                    await _persistence.CreatePictureAsync(picture);
                }
            }

            foreach (PictureDTO picture in ArticleToEdit.Pictures)
            {
                if (!article.Pictures.Contains(picture))
                {
                    await _persistence.DeletePictureAsync(picture.Id);
                }
            }

            ArticleToEdit = null;
            ArticleListElement articleToModify = _articleList.FirstOrDefault(a => a.Id == article.Id);
            articleToModify.Title = article.Title;
            articleToModify.LastModified = articleToSave.LastModified;
            OnArticleChanged(articleToModify);           
        }

        private void OnArticleChanged(ArticleListElement article)
        {
            if (ArticleChanged != null)
                ArticleChanged(this, new ArticleListEventArgs { Article = article });
        }

        private void OnArticleCreated(ArticleListElement article)
        {
            if (ArticleCreated != null)
                ArticleCreated(this, new ArticleListEventArgs { Article = article });
        }

        private void OnPictureCreated(PictureDTO picture)
        {
            if (PictureCreated != null)
                PictureCreated(this, new PictureEventArgs { Picture = picture });
        }

        

        private int GetNextId()
        {
            return --_generatedId;
        }

        private bool IsFormInvalid(ArticleDTO article)
        {
            return String.IsNullOrEmpty(article.Title)
                || String.IsNullOrEmpty(article.Summary)
                || article.Summary.Length > 1000
                || String.IsNullOrEmpty(article.Content)
                || (article.Lead == true && article.Pictures.Count == 0);
        }

        public async void Model_LoginSuccessAsync(object sender, EventArgs e)
        {
            UserDTO user = await _persistence.GetUser();
            RecentUser = user;
        }
    }
}
