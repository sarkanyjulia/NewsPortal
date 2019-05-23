using NUnit.Framework;
using Moq;
using NewsPortal.Persistence;
using NewsPortal.WebSite.Models;
using NewsPortal.WebSite.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace NewsPortal.WebSite.Test
{
    class ArchivesControllerTest
    {
        readonly Article testArticle1 = new Article() { Id = 1, Title = "Title1", Summary = "Summary1", Content = "This is article no.1.", Lead = true, UserId = 1 };
        readonly Article testArticle2 = new Article() { Id = 2, Title = "Title2", Summary = "Summary2", Content = "This is article no.2.", Lead = false, UserId = 1 };
        readonly User testUser = new User() { Id = 1, Name = "TestUser" };
        readonly List<Article> testArticleList = new List<Article>()
        {
            new Article(), new Article(), new Article(), new Article(), new Article(),
            new Article(), new Article(), new Article(), new Article(), new Article(),
            new Article(), new Article(), new Article(), new Article(), new Article(),
            new Article(), new Article(), new Article(), new Article(), new Article(),
            new Article(), new Article(), new Article(), new Article(), new Article()
        };

        [Test]
        public void Index_WithoutPageNumber()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetOrderedArticles()).Returns(testArticleList.AsQueryable());           
            ArchivesController underTest = new ArchivesController(mockService.Object);
            // WHEN
            var result = underTest.Index(null);
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model as PaginatedList<Article>;
            Assert.AreEqual("Index", viewResult.ViewName);
            Assert.AreEqual(2, model.TotalPages);
            Assert.AreEqual(1, model.PageIndex);
        }

        [Test]
        public void Index_WithPageNumber()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.GetOrderedArticles()).Returns(testArticleList.AsQueryable());
            ArchivesController underTest = new ArchivesController(mockService.Object);
            // WHEN
            var result = underTest.Index(2);
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model as PaginatedList<Article>;
            Assert.AreEqual("Index", viewResult.ViewName);
            Assert.AreEqual(2, model.TotalPages);
            Assert.AreEqual(2, model.PageIndex);
        }

        [Test]
        public void Search()
        {
            // GIVEN
            var mockService = new Mock<INewsPortalService>();          
            ArchivesController underTest = new ArchivesController(mockService.Object);
            // WHEN
            var result = underTest.Search();
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model as SearchPageViewModel;
            Assert.AreEqual("Search", viewResult.ViewName);
            Assert.IsNull(model.DateFrom);
            Assert.IsNull(model.DateTo);
            Assert.IsNull(model.Title);
            Assert.IsNull(model.Content);
            Assert.IsNull(model.Result);
        }

        [Test]
        public void Result_WithoutPageNumber()
        {
            // GIVEN
            DateTime testDate = new DateTime(2000,1,1);
            String testString = "abc";
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.FindArticles(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>())).Returns(testArticleList.AsQueryable());
            ArchivesController underTest = new ArchivesController(mockService.Object);
            // WHEN
            var result = underTest.Result(testDate, testDate, testString, testString, null);
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model as SearchPageViewModel;
            Assert.AreEqual("Search", viewResult.ViewName);
            Assert.NotNull(model.Result);
            Assert.AreEqual(2, model.Result.TotalPages);
            Assert.AreEqual(1, model.Result.PageIndex);
        }

        [Test]
        public void Result_WithPageNumber()
        {
            // GIVEN
            DateTime testDate = new DateTime(2000, 1, 1);
            String testString = "abc";
            var mockService = new Mock<INewsPortalService>();
            mockService.Setup(s => s.FindArticles(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>())).Returns(testArticleList.AsQueryable());
            ArchivesController underTest = new ArchivesController(mockService.Object);
            // WHEN
            var result = underTest.Result(testDate, testDate, testString, testString, 2);
            // THEN
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model as SearchPageViewModel;
            Assert.AreEqual("Search", viewResult.ViewName);
            Assert.NotNull(model.Result);
            Assert.AreEqual(2, model.Result.TotalPages);
            Assert.AreEqual(2, model.Result.PageIndex);
        }
    }
}
