using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsPortal.Data;
using NewsPortal.Persistence;
using Microsoft.AspNetCore.Identity;

namespace NewsPortal.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly NewsPortalContext _context;

        public ArticlesController(NewsPortalContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public IActionResult GetArticles()
        {
            //return _context.Articles.OrderByDescending(article => article.LastModified);
            try
            {
                return Ok(_context.Articles.Include(a => a.Author).OrderByDescending(article => article.LastModified)
                    .ToList()
                    .Select(article => new ArticleListElement
                    {
                        Id = article.Id,
                        Title = article.Title,
                        LastModified = article.LastModified,
                        AuthorName = article.Author.Name
                    }));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public IActionResult GetArticle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = _context.Articles.Find(id);
            var pictures = _context.Pictures.Where(p => p.ArticleId == id)
                .Select(image => new PictureDTO { Id = image.Id, ArticleId = image.ArticleId, ImageSmall = image.ImageSmall, ImageLarge = null });

            if (article == null)
            {
                return NotFound();
            }
            ArticleDTO articleDTO = new ArticleDTO
            {
                Id = article.Id,
                Title = article.Title,
                LastModified = article.LastModified,
                Summary = article.Summary,
                Content = article.Content,
                Lead = article.Lead,
                UserId = article.UserId,
                Pictures = pictures.ToList()             
            };
            return Ok(articleDTO);
        }

        // PUT: api/Articles/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult PutArticle([FromRoute] int id, [FromBody] ArticleDTO articleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != articleDTO.Id)
            {
                return BadRequest();
            }
            
            try
            {
                Article article = _context.Articles.FirstOrDefault(a => a.Id == articleDTO.Id);
                if (article == null)
                    return NotFound();

                article.Title = articleDTO.Title;
                article.LastModified = articleDTO.LastModified;
                article.Summary = articleDTO.Summary;
                article.Content = articleDTO.Content;
                article.Lead = articleDTO.Lead;
                
                _context.SaveChanges();
                return Ok();
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }        
        }

        // POST: api/Articles
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult PostArticle([FromBody] ArticleDTO articleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var addedArticle = _context.Articles.Add(new Article
                {
                    Title = articleDTO.Title,
                    LastModified = articleDTO.LastModified,
                    Summary = articleDTO.Summary,
                    Content = articleDTO.Content,
                    Lead = articleDTO.Lead,
                    UserId = articleDTO.UserId
                });
                _context.SaveChanges(); 
                articleDTO.Id = addedArticle.Entity.Id;

                foreach (PictureDTO pictureDTO in articleDTO.Pictures)
                {
                    _context.Pictures.Add(new Picture
                    {
                        ArticleId = articleDTO.Id,
                        ImageSmall = pictureDTO.ImageSmall,
                        ImageLarge = pictureDTO.ImageLarge
                    });
                }
                _context.SaveChanges();

                return CreatedAtAction("GetArticle", new { id = addedArticle.Entity.Id }, articleDTO);

            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteArticle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Article article = _context.Articles.FirstOrDefault(a => a.Id == id);

                if (article == null) // ha nincs ilyen azonosító, akkor hibajelzést küldünk
                    return NotFound();

                _context.Articles.Remove(article);
                _context.SaveChanges(); 
                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}