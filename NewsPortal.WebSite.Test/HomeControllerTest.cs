using NUnit.Framework;
using Moq;
using NewsPortal.Persistence;
using NewsPortal.WebSite.Models;
using NewsPortal.WebSite.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace NewsPortal.WebSite.Test
{
    public class HomeControllerTest
    {
        readonly Article testArticle1 = new Article() { Id = 1, Title = "Title1", Summary = "Summary1", Content = "This is article no.1.", Lead = true, UserId=1 };
        readonly Article testArticle2 = new Article() { Id = 2, Title = "Title2", Summary = "Summary2", Content = "This is article no.2.", Lead = false, UserId=1 };
        readonly User testUser = new User() { Id = 1, Name = "TestUser" };
        readonly byte[] testByteArray = new byte[] { 0, 0, 0 };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Index_Ok()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetLeadingArticle()).Returns(testArticle1);
            mockService.Setup(s => s.GetFreshArticles()).Returns(new List<Article>() { testArticle2 });
            HomeController underTest = new HomeController(mockService.Object);
            // WHEN
            var result = underTest.Index();
            // THEN
            Assert.NotNull(result);           
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model as HomePageViewModel;
            Assert.AreEqual("Index", viewResult.ViewName);
            Assert.NotNull(model.LeadingArticle);
            Assert.True(model.Articles.Count==1);
        }

        [Test]
        public void Index_NoFreshArticles()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetLeadingArticle()).Returns(testArticle1);
            mockService.Setup(s => s.GetFreshArticles()).Returns(new List<Article>());
            HomeController underTest = new HomeController(mockService.Object);
            // WHEN
            var result = underTest.Index();
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model as HomePageViewModel;
            Assert.AreEqual("Index", viewResult.ViewName);
            Assert.NotNull(model.LeadingArticle);
            Assert.True(model.Articles.Count == 0);
        }

        [Test]
        public void Index_NoLeadingArticle()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetLeadingArticle()).Returns(null as Article);
            mockService.Setup(s => s.GetFreshArticles()).Returns(new List<Article>() { testArticle2 });
            HomeController underTest = new HomeController(mockService.Object);
            // WHEN
            var result = underTest.Index();
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model as HomePageViewModel;
            Assert.AreEqual("Index", viewResult.ViewName);
            Assert.Null(model.LeadingArticle);
            Assert.True(model.Articles.Count == 1);
        }

        [Test]
        public void Article_Exists()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetArticle(1)).Returns(testArticle1);            
            HomeController underTest = new HomeController(mockService.Object);
            // WHEN
            var result = underTest.Article(1);
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Article", viewResult.ViewName);
        }

        [Test]
        public void Article_NotExists()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetArticle(1)).Returns(null as Article);
            HomeController underTest = new HomeController(mockService.Object);
            // WHEN
            var result = underTest.Article(1);
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Article", viewResult.ViewName);
        }

        [Test]
        public void MainPictureForArticle_Exists()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetMainImage(It.IsAny<int>())).Returns(testByteArray);
            HomeController underTest = new HomeController(mockService.Object);
            // WHEN
            var result = underTest.MainPictureForArticle(1);
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<FileResult>(result);
        }

        [Test]
        public void MainPictureForArticle_NotExists()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetMainImage(It.IsAny<int>())).Returns(null as byte[]);
            HomeController underTest = new HomeController(mockService.Object);
            // WHEN
            var result = underTest.MainPictureForArticle(1);
            // THEN
            Assert.Null(result);           
        }

        [Test]
        public void LargePicture_Exists()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetLargePictureById(It.IsAny<int>())).Returns(testByteArray);
            HomeController underTest = new HomeController(mockService.Object);
            // WHEN
            var result = underTest.LargePicture(1);
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<FileResult>(result);
        }

        [Test]
        public void LargePicture_NotExists()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetLargePictureById(It.IsAny<int>())).Returns(null as byte[]);
            HomeController underTest = new HomeController(mockService.Object);
            // WHEN
            var result = underTest.LargePicture(1);
            // THEN
            Assert.Null(result);
        }

        [Test]
        public void Gallery()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetPictureIds(It.IsAny<int>())).Returns( new List<int>() { 1, 2, 3 });
            HomeController underTest = new HomeController(mockService.Object);
            // WHEN
            var result = underTest.Gallery(1);
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model as GalleryViewModel;
            Assert.AreEqual("Gallery", viewResult.ViewName);
            Assert.AreEqual(1, model.ArticleId);
            Assert.AreEqual(3, model.PictureIds.Count);
        }
    }
}