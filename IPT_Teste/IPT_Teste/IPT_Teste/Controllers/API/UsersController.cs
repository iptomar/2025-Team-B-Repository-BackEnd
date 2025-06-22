using IPT_Teste.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UsersController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<object>();

            foreach (var user in users)
            {
                result.Add(new
                {
                    user.Id, // Include ID for client operations
                    user.UserName,
                    user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            return Ok(result);
        }

        // GET: api/users/roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetAvailableRoles()
        {
            var roles = await _roleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();

            return Ok(roles);
        }

        // PUT: api/users/{userId}
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserModel model)
        {
            // Validate input
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            // Validate roles exist
            foreach (var role in model.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    return BadRequest($"Role '{role}' does not exist");
            }

            // Start transaction
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Update email and username
                    user.Email = model.Email;
                    user.UserName = model.Username;

                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(updateResult.Errors);
                    }

                    // Update roles
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(removeResult.Errors);
                    }

                    var addResult = await _userManager.AddToRolesAsync(user, model.Roles);
                    if (!addResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(addResult.Errors);
                    }

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync(); 
                    throw; // Re-throw exception for global error handling
                }
            }

            return Ok(user);
        }

        // Updated model for user edits
        public class UpdateUserModel
        {
            [Required(ErrorMessage = "At least one role is required")]
            public List<string> Roles { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Username is required")]
            public string Username { get; set; }
        }
    }
}