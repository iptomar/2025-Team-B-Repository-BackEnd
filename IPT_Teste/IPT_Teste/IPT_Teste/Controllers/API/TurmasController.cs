using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Models;
using IPT_Teste.Data;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurmasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TurmasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Turmas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turmas>>> GetTurmas()
        {
            return await _context.Turmas.Include(t => t.Curso).ToListAsync();
        }

        // GET: api/Turmas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Turmas>> GetTurma(int id)
        {
            var turma = await _context.Turmas.Include(t => t.Curso)
                                             .FirstOrDefaultAsync(t => t.Id == id);

            if (turma == null)
            {
                return NotFound();
            }

            return turma;
        }

        // POST: api/Turmas
        [HttpPost]
        public async Task<ActionResult<Turmas>> PostTurma(Turmas turma)
        {
            _context.Turmas.Add(turma);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTurma", new { id = turma.Id }, turma);
        }

        // PUT: api/Turmas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTurma(int id, Turmas turma)
        {
            if (id != turma.Id)
            {
                return BadRequest();
            }

            _context.Entry(turma).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TurmaExists(id))
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

        // DELETE: api/Turmas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTurma(int id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null)
            {
                return NotFound();
            }

            _context.Turmas.Remove(turma);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TurmaExists(int id)
        {
            return _context.Turmas.Any(e => e.Id == id);
        }
    }
}