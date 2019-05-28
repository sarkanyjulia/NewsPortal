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
    public class UsersController : ControllerBase
    {
        private readonly NewsPortalContext _context;

        public UsersController(NewsPortalContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUser()
        {
            int userId = Int32.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _context.Users.Find(userId);
            return Ok(new UserDTO
            {
                Id = user.Id,
                Name = user.Name
            });
        }
    }
}
