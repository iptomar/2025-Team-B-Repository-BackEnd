using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Data;
using IPT_Teste.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CursosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cursos>>> GetCursos()
        {
            return await _context.Cursos
                .Include(c => c.AnoLetivo)
                .Include(c => c.Instituicao)
                .Include(c => c.Grau)
                .Include(c => c.Professor)
                .ToListAsync();
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cursos>> GetCurso(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.AnoLetivo)
                .Include(c => c.Instituicao)
                .Include(c => c.Grau)
                .Include(c => c.Professor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
            {
                return NotFound();
            }

            return curso;
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<Cursos>> PostCurso(Cursos curso)
        {
            var user = await _userManager.FindByIdAsync(curso.ProfessorFK);

            if (user == null || !await _userManager.IsInRoleAsync(user, "Coordenador de Curso"))
            {
                return BadRequest("ProfessorFK deve referir-se a um utilizador com o papel 'Coordenador de Curso'");
            }

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }

        // PUT: api/Cursos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Cursos curso)
        {
            if (id != curso.Id)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(curso.ProfessorFK);

            if (user == null || !await _userManager.IsInRoleAsync(user, "Coordenador de Curso"))
            {
                return BadRequest("ProfessorFK deve referir-se a um utilizador com o papel 'Coordenador de Curso'");
            }

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
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

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Cursos/coordenadores
        [HttpGet("coordenadores")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetCoordenadores()
        {
            var coordenadores = await _userManager.GetUsersInRoleAsync("Coordenador de Curso");
            return Ok(coordenadores);
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }

        // GET: api/Cursos/5/cadeiras
        [HttpGet("{id}/cadeiras")]
        public async Task<ActionResult<IEnumerable<Cadeiras>>> GetCadeirasDoCurso(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Cadeiras)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
            {
                return NotFound();
            }

            return Ok(curso.Cadeiras);
        }

        // POST: api/Cursos/5/cadeiras/10
        [HttpPost("{cursoId}/cadeiras/{cadeiraId}")]
        public async Task<IActionResult> AdicionarCadeiraAoCurso(int cursoId, int cadeiraId)
        {
            var curso = await _context.Cursos
                .Include(c => c.Cadeiras)
                .FirstOrDefaultAsync(c => c.Id == cursoId);
            var cadeira = await _context.Cadeiras.FindAsync(cadeiraId);

            if (curso == null || cadeira == null)
            {
                return NotFound("Curso ou cadeira não encontrados.");
            }

            if (curso.Cadeiras.Any(c => c.Id == cadeiraId))
            {
                return BadRequest("A cadeira já está associada ao curso.");
            }

            curso.Cadeiras.Add(cadeira);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Cursos/5/cadeiras/10
        [HttpDelete("{cursoId}/cadeiras/{cadeiraId}")]
        public async Task<IActionResult> RemoverCadeiraDoCurso(int cursoId, int cadeiraId)
        {
            var curso = await _context.Cursos
                .Include(c => c.Cadeiras)
                .FirstOrDefaultAsync(c => c.Id == cursoId);
            var cadeira = await _context.Cadeiras.FindAsync(cadeiraId);

            if (curso == null || cadeira == null)
            {
                return NotFound("Curso ou cadeira não encontrados.");
            }

            if (!curso.Cadeiras.Any(c => c.Id == cadeiraId))
            {
                return BadRequest("A cadeira não está associada ao curso.");
            }

            curso.Cadeiras.Remove(cadeira);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: api/Cursos/anoletivo/3
        [HttpGet("anoletivo/{anoLetivo}")]
        public async Task<ActionResult<IEnumerable<Cursos>>> GetCursosPorAnoLetivo(int anoLetivo)
        {
            return await _context.Cursos
                .Include(c => c.AnoLetivo)
                .Include(c => c.Instituicao)
                .Include(c => c.Grau)
                .Include(c => c.Professor)
                .Where(c => c.AnoLetivoFK == anoLetivo)
                .ToListAsync();
        }

        // GET: api/Cursos/grau/3
        [HttpGet("grau/{grauId}")]
        public async Task<ActionResult<IEnumerable<Cursos>>> GetCursosPorGrau(int grauId)
        {
            return await _context.Cursos
                .Include(c => c.AnoLetivo)
                .Include(c => c.Instituicao)
                .Include(c => c.Grau)
                .Include(c => c.Professor)
                .Where(c => c.GrauFK == grauId)
                .ToListAsync();
        }

    }
}
