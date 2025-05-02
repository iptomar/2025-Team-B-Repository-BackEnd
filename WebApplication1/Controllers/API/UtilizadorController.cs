using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UtilizadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Endpoint que Lista de Utilizadores da aplicação 
        /// (Recebe-se o utilizador atual para devolver todos exceto ele próprio)
        /// </summary>
        /// <returns></returns>
        [HttpGet("Lista_Utilizador")]
        public async Task<IActionResult> ListaUtilizadores()
        {
            var username = User.Identity?.Name;

            if (username == null)
            {
                return Unauthorized();
            }

            var userAtual = _context.Utilizadador
                                .Where(u => u.IDAspNetUser == username)
                                .Select(u => u.Nome)
                                .FirstOrDefault();

            var result = await _context.Utilizadador
                                .Join(_context.Users,
                                u => u.IDAspNetUser,
                                us => us.Id,
                                (u, us) => new
                                {
                                    Username = u.Nome,
                                    Email = us.Email,
                                })
                                .Where(u => u.Username != userAtual)
                                .Select(u => new
                                {
                                    Nome_Utilizador = u.Username,
                                    Email_Utilizador = u.Email
                                })
                                .ToListAsync();
            return Ok(result);
        }


    }
}
