using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsPortal.Data;
using NewsPortal.Persistence;

namespace NewsPortal.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class PicturesController : ControllerBase
    {
        private readonly NewsPortalContext _context;

        public PicturesController(NewsPortalContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _context = context;
        }     
   

        // POST: api/Pictures
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult PostPicture([FromBody] PictureDTO pictureDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pictureDTO == null || !_context.Articles.Any(a => pictureDTO.ArticleId == a.Id))
                return NotFound();

            int userId = GetUserId();
            Article article = _context.Articles.Where(a => a.Id == pictureDTO.ArticleId).FirstOrDefault();
            if (userId != article.UserId)
            {
                return BadRequest();
            }

            Picture picture = new Picture
            {
                ArticleId = pictureDTO.ArticleId,
                ImageSmall = pictureDTO.ImageSmall,
                ImageLarge = pictureDTO.ImageLarge
            };

            var addedPicture = _context.Pictures.Add(picture);

            try
            {
                _context.SaveChanges();
                pictureDTO.Id = addedPicture.Entity.Id;
                return CreatedAtAction("GetPicture", new { id = pictureDTO.Id }, pictureDTO.Id);
                //return Created(Request.GetUri() + pictureDTO.Id.ToString(), pictureDTO.Id); // csak az azonosítót küldjük vissza
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Pictures/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeletePicture([FromRoute] int id)
        {            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Picture picture = _context.Pictures.FirstOrDefault(pic => pic.Id == id);          
            if (picture == null)
                return NotFound();

            int userId = GetUserId();
            Article article = _context.Articles.Where(a => a.Id == picture.ArticleId).FirstOrDefault();
            if (userId != article.UserId)
            {
                return BadRequest();
            }

            try
            {
                _context.Pictures.Remove(picture);
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

                /*
         // Egy cikkhez tartozó képek
         [HttpGet("a/{articleId}")]
         [Authorize]
         public IActionResult GetImagesByArticle([FromRoute] int articleId)
         {
             int userId = Int32.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
             Article article = _context.Articles.Find(articleId);
             if (userId != article.UserId)
             {
                 return BadRequest();
             }

             return Ok(_context.Pictures.Where(p => p.ArticleId == articleId)
                 .Select(image => new PictureDTO { Id = image.Id, ArticleId = image.ArticleId, ImageSmall = image.ImageSmall, ImageLarge = null }));                   

         }

         // GET: api/Pictures/5
         [HttpGet("{id}")]
         [Authorize]
         public IActionResult GetPicture([FromRoute] int id)
         {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }

             Picture picture = _context.Pictures.Find(id);

             if (picture == null)
                 return NotFound();

             int userId = Int32.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
             Article article = _context.Articles.Where(a => a.Id == picture.ArticleId).FirstOrDefault();
             if (userId != article.UserId)
             {
                 return BadRequest();
             }

             return Ok(new PictureDTO
             {
                 Id = picture.Id,
                 ArticleId = picture.ArticleId,
                 ImageSmall = picture.ImageSmall,
                 ImageLarge = picture.ImageLarge
             });
         }*/

        private bool PictureExists(int id)
        {
            return _context.Pictures.Any(e => e.Id == id);
        }
        protected virtual int GetUserId()
        {
            return Int32.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
      
    }
}