using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApplication1.Data;
using WebApplication1.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspNetUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AspNetUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> SelectUserName()
        {
            var nome = _context.UserManager.
                Select(u => u.UserName)
                .ToListAsync();

            return Ok(nome);
        }

        // https://localhost:7110/api/AspNetUsers/SelectNameProfessor

        [HttpGet("SelectNameProfessor")]
        public async Task<IActionResult> SelectUserNameProfessorCoordenador()
        {
            var nome = await _context.UserManager
                .Join(_context.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new { user, userRole })
                .Join(_context.Roles,
                    combined => combined.userRole.RoleId,
                    role => role.Id,
                    (combined, role) => new
                    {
                        UserName = combined.user.UserName,
                        RoleId = role.Id,
                        RoleName = role.Name
                    })
                .Where(x => x.RoleId == "c0f0653e-b870-4c0a-811f-7fe82f0e8755")
                .Select(x => new
                {
                    ProfessorCoordenador = x.UserName
                })
                .ToListAsync();

            return Ok(nome);
        }
    }
}
