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
    public class AnosLetivosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AnosLetivosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AnoLetivos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnosLetivos>>> GetAnoLetivos()
        {
            return await _context.AnosLetivos.ToListAsync();
        }

        // GET: api/AnoLetivos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnosLetivos>> GetAnoLetivo(int id)
        {
            var anoLetivo = await _context.AnosLetivos.FindAsync(id);

            if (anoLetivo == null)
            {
                return NotFound();
            }

            return anoLetivo;
        }

        // POST: api/AnoLetivos
        [HttpPost]
        public async Task<ActionResult<AnosLetivos>> PostAnoLetivo(AnosLetivos anoLetivo)
        {
            _context.AnosLetivos.Add(anoLetivo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnoLetivo), new { id = anoLetivo.Id }, anoLetivo);
        }

        // PUT: api/AnoLetivos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnoLetivo(int id, AnosLetivos anoLetivo)
        {
            if (id != anoLetivo.Id)
            {
                return BadRequest();
            }

            _context.Entry(anoLetivo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnoLetivoExists(id))
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

        // DELETE: api/AnoLetivos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnoLetivo(int id)
        {
            var anoLetivo = await _context.AnosLetivos.FindAsync(id);
            if (anoLetivo == null)
            {
                return NotFound();
            }

            _context.AnosLetivos.Remove(anoLetivo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnoLetivoExists(int id)
        {
            return _context.AnosLetivos.Any(e => e.Id == id);
        }
    }
}