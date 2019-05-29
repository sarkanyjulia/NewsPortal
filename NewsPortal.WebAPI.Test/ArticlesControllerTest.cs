using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NewsPortal.Persistence;
using NewsPortal.WebAPI.Controllers;
using NewsPortal.WebAPI.Test;
using NUnit.Framework;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NewsPortal.Data;
using System.Linq;

namespace NewsPortal.WebAPI.Tests
{
    public class ArticlesControllerTests
    {
        private NewsPortalContext _context { get; set; }     

        readonly Article testArticle1 = new Article() { Id = 1, Title = "Title", Summary = "Summary", Content = "This is article no.1.", LastModified = new DateTime(2000, 1, 1), Lead = true, UserId = 1 };
        readonly Article testArticle2 = new Article() { Id = 2, Title = "Title", Summary = "Summary", Content = "This is article no.2.", LastModified = new DateTime(2000, 1, 1), Lead = false, UserId = 1 };
        readonly Article testArticle3 = new Article() { Id = 3, Title = "Title", Summary = "Summary", Content = "This is article no.3.", LastModified = new DateTime(2000, 1, 1), Lead = false, UserId = 2 };
        readonly ArticleDTO testArticleDTO = new ArticleDTO() { Id = 1, Title = "NewTitle", Summary = "Summary", Content = "This is article no.1.", LastModified = new DateTime(2000, 1, 1), Lead = false, UserId = 1 };
        readonly ArticleDTO newTestArticleDTO = new ArticleDTO() { Id=0, Title = "NewTitle", Summary = "Summary", Content = "This is article no.1.", LastModified = new DateTime(2000, 1, 1), Lead = false, UserId = 1, Pictures = new List<PictureDTO>() };
        readonly User testUser = new User() { UserName="user", Name = "User" };
        readonly User testUser2 = new User() { UserName = "user2", Name = "User2" };

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NewsPortalContext>()
                .UseInMemoryDatabase("NewsPortalTest")
                .Options;

            _context = new NewsPortalContext(options);           
            _context.Database.EnsureCreated();

            _context.Add(testUser);
            _context.Add(testUser2);
            _context.Add(testArticle1);
            _context.Add(testArticle2);
            _context.Add(testArticle3);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void TestGetArticles()
        {
            // GIVEN
            ArticlesController underTest = new TestableArticlesController(_context);
            // WHEN
            var result = underTest.GetArticles();
            // THEN
            Assert.IsNotNull(result);          
            var okResult = result as OkObjectResult; 
            Assert.IsInstanceOf<OkObjectResult>(okResult, result.GetType().ToString());

            var collectionResult = okResult.Value as IEnumerable<ArticleListElement>;            
            Assert.IsInstanceOf<IEnumerable<ArticleListElement>>(collectionResult, result.GetType().ToString());

            Assert.AreEqual(2, collectionResult.Count());           
        }

        [Test]
        public void TestGetArticleById_Ok()
        {
            // GIVEN
            ArticlesController underTest = new TestableArticlesController(_context);
            // WHEN
            var result = underTest.GetArticle(1);
            // THEN
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<OkObjectResult>(okResult);
            var articleResult = okResult.Value as ArticleDTO;
            Assert.IsNotNull(articleResult);
            Assert.IsTrue(1 == articleResult.Id);
        }

        [Test]
        public void TestGetArticleById_NotFound()
        {
            // GIVEN
            ArticlesController underTest = new TestableArticlesController(_context);
            // WHEN
            var result = underTest.GetArticle(5);
            // THEN
            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(notFoundResult, result.GetType().ToString());          
        }

        [Test]
        public void TestGetArticleById_Forbid()
        {
            // GIVEN
            ArticlesController underTest = new TestableArticlesController(_context);
            // WHEN
            var result = underTest.GetArticle(3);
            // THEN
            Assert.IsNotNull(result);            
            var forbidResult = result as ForbidResult;
            Assert.IsInstanceOf<ForbidResult>(forbidResult, result.GetType().ToString());
        }

        [Test]
        public void TestPutArticle()
        {
            // GIVEN
            ArticlesController underTest = new TestableArticlesController(_context);
            // WHEN
            var result = underTest.PutArticle(1, testArticleDTO);
            // THEN
            Assert.IsNotNull(result);
            var okResult = result as OkResult;
            Assert.IsInstanceOf<OkResult>(okResult, result.GetType().ToString());
        }

        [Test]
        public void TestDeleteArticle_Ok()
        {
            // GIVEN
            ArticlesController underTest = new TestableArticlesController(_context);
            // WHEN
            var result = underTest.DeleteArticle(1);
            // THEN
            Assert.IsNotNull(result);
            var okResult = result as OkResult;
            Assert.IsInstanceOf<OkResult>(okResult, result.GetType().ToString());
            Assert.IsNull(_context.Articles.Find(1));
        }

        [Test]
        public void TestDeleteArticle_NotFound()
        {
            // GIVEN
            ArticlesController underTest = new TestableArticlesController(_context);
            // WHEN
            var result = underTest.DeleteArticle(5);
            // THEN
            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(notFoundResult, result.GetType().ToString());
            Assert.AreEqual(3, _context.Articles.Count());
        }

        [Test]
        public void TestDeleteArticle_Forbidden()
        {
            // GIVEN
            ArticlesController underTest = new TestableArticlesController(_context);
            // WHEN
            var result = underTest.DeleteArticle(3);
            // THEN
            Assert.IsNotNull(result);
            var forbiddenResult = result as ForbidResult;
            Assert.IsInstanceOf<ForbidResult>(forbiddenResult, result.GetType().ToString());
            Assert.AreEqual(3, _context.Articles.Count());
        }

        
        public void TestPostArticle()
        {
            // GIVEN
            ArticlesController underTest = new TestableArticlesController(_context);
            // WHEN
            var result = underTest.PostArticle(newTestArticleDTO);
            // THEN
            Assert.IsNotNull(result);
            //Assert.AreEqual(4, _context.Articles.Count());
            var okResult = result as CreatedAtActionResult;
            Assert.IsInstanceOf<CreatedAtActionResult>(okResult, result.GetType().ToString());
        }
    }
}