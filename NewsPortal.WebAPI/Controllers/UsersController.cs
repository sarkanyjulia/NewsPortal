using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;

        public UsersController(NewsPortalContext context, UserManager<User> userManager)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {          
            User user = await _userManager.GetUserAsync(User);
            return Ok(new UserDTO
            {
                Id = user.Id,
                Name = user.Name
            });
        }
    }
}
