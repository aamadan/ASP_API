using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;  //Identity
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;  //Claims
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public DepartmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        // GET: api/department
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetDepartments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var departmentsQuery = _context.Departments.AsQueryable();

            // Regular users only see their own departments. Admins see all.
            if (!User.IsInRole("Admin"))
            {
                departmentsQuery = departmentsQuery.Where(d => d.ApplicationUserId == userId);
            }

            var departments = await departmentsQuery
                .Include(d => d.ApplicationUser)
                .Select(d => new {
                    d.Id,
                    d.Name,
                    d.Description,
                    d.ApplicationUserId,
                    UserName = d.ApplicationUser.UserName
                })
                .ToListAsync();

            return Ok(departments);
        }

        // GET: api/department/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            // Check authorization using the policy
            var authResult = await _authorizationService.AuthorizeAsync(User, department, "SameOwnerPolicy");
            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            return department;
        }

        // POST: api/department
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            // Assign the new department to the currently logged-in user.
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            department.ApplicationUserId = userId;

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
        }

        // PUT: api/department/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }

            var existingDepartment = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            if (existingDepartment == null)
            {
                return NotFound();
            }

            // Check authorization using the policy
            var authResult = await _authorizationService.AuthorizeAsync(User, existingDepartment, "SameOwnerPolicy");
            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            // Ensure the original owner ID is preserved.
            department.ApplicationUserId = existingDepartment.ApplicationUserId;
            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/department/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            // Check authorization using the policy
            var authResult = await _authorizationService.AuthorizeAsync(User, department, "SameOwnerPolicy");
            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
