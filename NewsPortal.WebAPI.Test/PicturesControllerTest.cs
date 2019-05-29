using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsPortal.Data;
using NewsPortal.Persistence;
using NewsPortal.WebAPI.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsPortal.WebAPI.Test
{
    public class PicturesControllerTest
    {
        private NewsPortalContext _context { get; set; }

        readonly User testUser = new User() { UserName = "user", Name = "User" };
        readonly User testUser2 = new User() { UserName = "user2", Name = "User2" };
        readonly Article testArticle1 = new Article() { Id = 1, Title = "Title", Summary = "Summary", Content = "This is article no.1.", LastModified = new DateTime(2000, 1, 1), Lead = false, UserId = 1 };
        readonly Picture testPicture = new Picture() { Id = 1, ArticleId = 1, ImageSmall = new byte[] { }, ImageLarge = new byte[] { } };
        readonly PictureDTO testPictureDTO = new PictureDTO() { Id = 2, ArticleId = 1, ImageSmall = new byte[] { }, ImageLarge = new byte[] { } };

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
            //_context.Add(testPicture);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void TestDeletePicture_Ok()
        {
            // GIVEN
            _context.Add(testPicture);
            _context.SaveChanges();
            PicturesController underTest = new TestablePicturesController(_context);
            // WHEN
            var result = underTest.DeletePicture(1);
            // THEN
            Assert.IsNotNull(result);          
            var okResult = result as OkResult;
            Assert.IsInstanceOf<OkResult>(okResult, result.GetType().ToString());
            Assert.AreEqual(0, _context.Pictures.Count());
        }

        [Test]
        public void TestDeletePicture_NotFound()
        {
            // GIVEN
            _context.Add(testPicture);
            _context.SaveChanges();
            PicturesController underTest = new TestablePicturesController(_context);
            // WHEN
            var result = underTest.DeletePicture(5);
            // THEN
            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(notFoundResult, result.GetType().ToString());
            Assert.AreEqual(1, _context.Pictures.Count());
        }

        [Test]
        public void TestPostPicture()
        {
            // GIVEN
            PicturesController underTest = new TestablePicturesController(_context);
            // WHEN
            var result = underTest.PostPicture(testPictureDTO);
            // THEN
            Assert.IsNotNull(result);
            Assert.AreEqual(1, _context.Pictures.Count());
            
            var okResult = result as CreatedAtActionResult;
            Assert.IsInstanceOf<CreatedAtActionResult>(okResult, result.GetType().ToString());           
        }

    }
}
