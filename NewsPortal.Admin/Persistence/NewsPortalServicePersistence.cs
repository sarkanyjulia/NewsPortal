using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Data;
using Newtonsoft.Json;

namespace NewsPortal.Admin.Persistence
{
    public class NewsPortalServicePersistence : INewsPortalPersistence
    {
        private HttpClient _client;

        public NewsPortalServicePersistence(String baseAddress)
        {
            _client = new HttpClient(); // a szolgáltatás kliense
            _client.BaseAddress = new Uri(baseAddress); // megadjuk neki a címet
        }
        public async Task<bool> CreateArticleAsync(ArticleDTO article)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/Articles/", article); // az értékeket azonnal JSON formátumra alakítjuk
                article.Id = (await response.Content.ReadAsAsync<ArticleDTO>()).Id; // a válaszüzenetben megkapjuk a végleges azonosítót
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> CreatePictureAsync(PictureDTO image)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/pictures/", image); // elküldjük a képet
                if (response.IsSuccessStatusCode)
                {
                    image.Id = await response.Content.ReadAsAsync<Int32>(); // a válaszüzenetben megkapjuk az azonosítót
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> DeleteArticleAsync(int articleId)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/articles/" + articleId);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> DeletePictureAsync(int pictureId)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/pictures/" + pictureId);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<UserDTO> GetUser()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/users/user");
                if (response.IsSuccessStatusCode)
                {
                    UserDTO user = await response.Content.ReadAsAsync<UserDTO>();
                    return user;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> LoginAsync(string userName, string userPassword)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/account/login/" + userName + "/" + userPassword);              
                return response.IsSuccessStatusCode; // a művelet eredménye megadja a bejelentkezés sikeressségét
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/account/logout");
                return !response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<ArticleDTO> ReadArticleAsync(int articleId)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/articles/" + articleId);
                if (response.IsSuccessStatusCode)
                {
                    ArticleDTO article = await response.Content.ReadAsAsync<ArticleDTO>();
                    return article;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<IEnumerable<ArticleListElement>> ReadArticlesAsync()
        {
            try
            {             
                HttpResponseMessage response = await _client.GetAsync("api/articles/");
                if (response.IsSuccessStatusCode) 
                {                   
                    IEnumerable<ArticleListElement> articles = await response.Content.ReadAsAsync<IEnumerable<ArticleListElement>>();                 
                    return articles;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {              
                throw new PersistenceUnavailableException(ex);
            }

        }

        

        public async Task<bool> UpdateArticleAsync(ArticleDTO article)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/articles/"+article.Id, article);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        
    }
}
