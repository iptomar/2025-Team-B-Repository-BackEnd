using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Models;
using IPT_Teste.Data;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AulasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AulasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Aulas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aulas>>> GetAulas()
        {
            var aulas = await _context.Aulas
                .Include(a => a.Cadeira)      
                .Include(a => a.Tipologia)   
                .Include(a => a.Turma)       
                .Include(a => a.Professor)  
                .ToListAsync();

            return Ok(aulas);
        }

        // GET: api/Aulas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aulas>> GetAulas(int id)
        {
            var aula = await _context.Aulas
                .Include(a => a.Cadeira)
                .Include(a => a.Tipologia)
                .Include(a => a.Turma)
                .Include(a => a.Professor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aula == null)
            {
                return NotFound();
            }

            return Ok(aula);
        }
        // GET: api/Aulas/5
        [HttpGet("Turma/{id}")]
        public async Task<ActionResult<Aulas>> GetAulasPorTurma(int id)
        {
            var aula = await _context.Aulas
                .Include(a => a.Cadeira)
                .Include(a => a.Tipologia)
                .Include(a => a.Turma)
                .Include(a => a.Professor)
                .Where(a => a.TurmaFK == id)
                .ToListAsync();

            if (aula == null)
            {
                return NotFound();
            }

            return Ok(aula);
        }

        // POST: api/Aulas
        [HttpPost]
        public async Task<ActionResult<Aulas>> PostAulas(Aulas aulas)
        {
            var user = await _userManager.FindByIdAsync(aulas.ProfessorFK);

            if (user == null || !await _userManager.IsInRoleAsync(user, "Docente"))
            {
                return BadRequest("ProfessorFK deve referir-se a um utilizador com o papel 'Docente'");
            }

            _context.Aulas.Add(aulas);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAulas), new { id = aulas.Id }, aulas);
        }

        // PUT: api/Aulas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAulas(int id, Aulas aulas)
        {
            if (id != aulas.Id)
            {
                return BadRequest();
            }

            _context.Entry(aulas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AulasExists(id))
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

        // DELETE: api/Aulas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAulas(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);
            if (aula == null)
            {
                return NotFound();
            }

            _context.Aulas.Remove(aula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AulasExists(int id)
        {
            return _context.Aulas.Any(e => e.Id == id);
        }
    }
}