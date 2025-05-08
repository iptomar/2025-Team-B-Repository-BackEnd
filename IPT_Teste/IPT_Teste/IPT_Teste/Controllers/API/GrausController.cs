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
    public class GrausController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GrausController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Graus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Graus>>> GetGraus()
        {
            return await _context.Graus.ToListAsync();
        }

        // GET: api/Graus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Graus>> GetGrau(int id)
        {
            var grau = await _context.Graus.FindAsync(id);

            if (grau == null)
            {
                return NotFound();
            }

            return grau;
        }

        // POST: api/Graus
        [HttpPost]
        public async Task<ActionResult<Graus>> PostGrau(Graus grau)
        {
            _context.Graus.Add(grau);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGrau), new { id = grau.Id }, grau);
        }

        // PUT: api/Graus/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrau(int id, Graus grau)
        {
            if (id != grau.Id)
            {
                return BadRequest();
            }

            _context.Entry(grau).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrauExists(id))
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

        // DELETE: api/Graus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrau(int id)
        {
            var grau = await _context.Graus.FindAsync(id);
            if (grau == null)
            {
                return NotFound();
            }

            _context.Graus.Remove(grau);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GrauExists(int id)
        {
            return _context.Graus.Any(e => e.Id == id);
        }
    }
}