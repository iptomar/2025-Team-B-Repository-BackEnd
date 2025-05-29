using IPT_Teste.Data;
using IPT_Teste.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPT_Teste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfessoresController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Professores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetAllDocentes()
        {
            var allUsers = _userManager.Users.ToList();
            var docentes = new List<IdentityUser>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Docente"))
                {
                    docentes.Add(user);
                }
            }

            return Ok(docentes);
        }

        // GET: api/Professores/curso/5
        [HttpGet("curso/{cursoId}")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetProfessoresByCurso(int cursoId)
        {
            var professores = await _context.Professores
                .Where(p => p.CursoFK == cursoId)
                .Include(p => p.Professor)
                .Select(p => p.Professor)
                .ToListAsync();

            return Ok(professores);
        }

        // POST: api/Professores
        [HttpPost]
        public async Task<ActionResult> AddProfessorToCurso([FromBody] Professores model)
        {
            var curso = await _context.Cursos.FindAsync(model.CursoFK);
            if (curso == null)
                return NotFound("Curso não encontrado");

            var user = await _userManager.FindByIdAsync(model.ProfessorFK);
            if (user == null)
                return NotFound("Usuário não encontrado");

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Docente"))
                return BadRequest("Usuário não tem o papel de docente");

            bool jaExiste = await _context.Professores
                .AnyAsync(p => p.CursoFK == model.CursoFK && p.ProfessorFK == model.ProfessorFK);
            if (jaExiste)
                return BadRequest("Esse docente já está associado a este curso");

            _context.Professores.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Docente associado ao curso com sucesso" });
        }


        // DELETE: api/Professores?cursoId=5&professorId=abc123
        [HttpDelete]
        public async Task<IActionResult> RemoveProfessorFromCurso([FromQuery] int cursoId, [FromQuery] string professorId)
        {
            var association = await _context.Professores
                .FirstOrDefaultAsync(p => p.CursoFK == cursoId && p.ProfessorFK == professorId);

            if (association == null)
                return NotFound("Associação não encontrada");

            _context.Professores.Remove(association);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Professor removido do curso com sucesso"});
        }
    }
}