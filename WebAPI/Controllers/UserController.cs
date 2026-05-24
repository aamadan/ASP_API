using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostUser([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { Status = "Error", Message = $"User with name {model.Username} already exists." });
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assign role if provided
            if (!string.IsNullOrEmpty(model.RoleName))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, model.RoleName);
                if (!roleResult.Succeeded)
                {
                    // If role assignment fails, it's best to delete the created user to avoid orphaned accounts
                    await _userManager.DeleteAsync(user);
                    return BadRequest(roleResult.Errors);
                }
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, ApplicationUser appUser)
        {
            if (id != appUser.Id)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = appUser.Email;
            user.UserName = appUser.UserName;
            // Update other properties as needed

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}
