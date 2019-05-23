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
        private enum DataFlag
        {
            Create,
            Update,
            Delete
        }

        private INewsPortalPersistence _persistence;
        private List<ArticleDTO> _articles;
        private List<ArticleListElement> _articleList;
        private Dictionary<ArticleDTO, DataFlag> _articleFlags;
        private Dictionary<PictureDTO, DataFlag> _imageFlags;
        

        public NewsPortalModel(INewsPortalPersistence persistence)
        {
            if (persistence == null)
                throw new ArgumentNullException(nameof(persistence));

            IsUserLoggedIn = false;
            _persistence = persistence;
        }

        public IReadOnlyList<ArticleDTO> Articles
        {
            get { return _articles; }
        }

        public List<ArticleListElement> ArticleList
        {
            get { return _articleList; }
        }
        public bool IsUserLoggedIn { get; private set; }

        public event EventHandler<ArticleEventArgs> ArticleChanged;

        public void CreateArticle(ArticleDTO article)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article));
            if (_articles.Contains(article))
                throw new ArgumentException("The article is already in the collection.", nameof(article));

            article.Id = (_articles.Count > 0 ? _articles.Max(b => b.Id) : 0) + 1; // generálunk egy új, ideiglenes azonosítót (nem fog átkerülni a szerverre)
            _articleFlags.Add(article, DataFlag.Create);
            _articles.Add(article);
        }

        public void CreatePicture(int articleId, byte[] imageSmall, byte[] imageLarge)
        {
            ArticleDTO article = _articles.FirstOrDefault(a => a.Id == articleId);
            if (article == null)
                throw new ArgumentException("The article does not exist.", nameof(articleId));

            // létrehozzuk a képet
            PictureDTO picture = new PictureDTO
            {
                Id = _articles.Max(a => a.Pictures.Any() ? a.Pictures.Max(im => im.Id) : 0) + 1,
                ArticleId = articleId,
                ImageSmall = imageSmall,
                ImageLarge = imageLarge
            };

            // hozzáadjuk
            article.Pictures.Add(picture);
            _imageFlags.Add(picture, DataFlag.Create);

            // jellezzük a változást
            //OnArticleChanged(article.Id);
        }

        public void DeleteArticle(ArticleDTO article)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article));

            // keresés azonosító alapján
            ArticleDTO articleToDelete = _articles.FirstOrDefault(a => a.Id == article.Id);

            if (articleToDelete == null)
                throw new ArgumentException("The article does not exist.", nameof(article));

            // külön kezeljük, ha egy adat újonnan hozzávett (ekkor nem kell törölni a szerverről)
            if (_articleFlags.ContainsKey(articleToDelete) && _articleFlags[articleToDelete] == DataFlag.Create)
                _articleFlags.Remove(articleToDelete);
            else
                _articleFlags[articleToDelete] = DataFlag.Delete;

            _articles.Remove(articleToDelete);
        }

        public void DeletePicture(PictureDTO picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            // megkeressük a képet
            foreach (ArticleDTO article in _articles)
            {
                if (!article.Pictures.Contains(picture))
                    continue;

                // kezeljük az állapotjelzéseket is
                if (_imageFlags.ContainsKey(picture))
                    _imageFlags.Remove(picture);
                else
                    _imageFlags.Add(picture, DataFlag.Delete);

                // töröljük a képet
                article.Pictures.Remove(picture);

                // jellezzük a változást
                //OnArticleChanged(article.Id);

                return;
            }

            // ha idáig eljutott, akkor nem sikerült képet törölni+
            throw new ArgumentException("The picture does not exist.", nameof(picture));
        }

        public async Task LoadAsync()
        {
            // adatok
            _articleList = (await _persistence.ReadArticlesAsync()).ToList();


            // állapotjelzések
            //_articleFlags = new Dictionary<ArticleDTO, DataFlag>();
            //_imageFlags = new Dictionary<PictureDTO, DataFlag>();
        }

        public async Task<bool> LoginAsync(string userName, string userPassword)
        {
            IsUserLoggedIn = await _persistence.LoginAsync(userName, userPassword);
            return IsUserLoggedIn;
        }

        public async Task<bool> LogoutAsync()
        {
            if (!IsUserLoggedIn)
                return true;

            IsUserLoggedIn = !(await _persistence.LogoutAsync());

            return IsUserLoggedIn;
        }

        public async Task SaveAsync()
        {
            // épületek
            List<ArticleDTO> articlesToSave = _articleFlags.Keys.ToList();

            foreach (ArticleDTO article in articlesToSave)
            {
                Boolean result = true;

                // az állapotjelzőnek megfelelő műveletet végezzük el
                switch (_articleFlags[article])
                {
                    case DataFlag.Create:
                        result = await _persistence.CreateArticleAsync(article);
                        break;
                    case DataFlag.Delete:
                        result = await _persistence.DeleteArticleAsync(article);
                        break;
                    case DataFlag.Update:
                        result = await _persistence.UpdateArticleAsync(article);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + _articleFlags[article] + " failed on article " + article.Id);

                // ha sikeres volt a mentés, akkor törölhetjük az állapotjelzőt
                _articleFlags.Remove(article);
            }

            // képek
            List<PictureDTO> imagesToSave = _imageFlags.Keys.ToList();

            foreach (PictureDTO image in imagesToSave)
            {
                Boolean result = true;

                switch (_imageFlags[image])
                {
                    case DataFlag.Create:
                        result = await _persistence.CreatePictureAsync(image);
                        break;
                    case DataFlag.Delete:
                        result = await _persistence.DeletePictureAsync(image);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + _imageFlags[image] + " failed on image " + image.Id);

                // ha sikeres volt a mentés, akkor törölhetjük az állapotjelzőt
                _imageFlags.Remove(image);
            }
        }

        public void UpdateArticle(ArticleDTO article)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article));

            // keresés azonosító alapján
            ArticleDTO articleToModify = _articles.FirstOrDefault(a => a.Id == article.Id);

            if (articleToModify == null)
                throw new ArgumentException("The article does not exist.", nameof(article));

            // módosítások végrehajtása
            articleToModify.Title = article.Title;
            articleToModify.LastModified = article.LastModified;
            articleToModify.Summary = article.Summary;
            articleToModify.Content = article.Content;
            articleToModify.Lead = article.Lead;
            articleToModify.UserId = article.UserId;

            // külön állapottal jelezzük, ha egy adat újonnan hozzávett
            if (_articleFlags.ContainsKey(articleToModify) && _articleFlags[articleToModify] == DataFlag.Create)
            {
                _articleFlags[articleToModify] = DataFlag.Create;
            }
            else
            {
                _articleFlags[articleToModify] = DataFlag.Update;
            }

            // jelezzük a változást
            //OnArticleChanged(article.Id);
        }
    }
}
