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
    public class CadeirasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CadeirasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cadeiras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cadeiras>>> GetCadeiras()
        {
            return await _context.Cadeiras.ToListAsync();
        }

        // GET: api/Cadeiras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cadeiras>> GetCadeira(int id)
        {
            var cadeira = await _context.Cadeiras.FindAsync(id);

            if (cadeira == null)
            {
                return NotFound();
            }

            return cadeira;
        }

        // POST: api/Cadeiras
        [HttpPost]
        public async Task<ActionResult<Cadeiras>> PostCadeira(Cadeiras cadeira)
        {
            _context.Cadeiras.Add(cadeira);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCadeira), new { id = cadeira.Id }, cadeira);
        }

        // PUT: api/Cadeiras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCadeira(int id, Cadeiras cadeira)
        {
            if (id != cadeira.Id)
            {
                return BadRequest();
            }

            _context.Entry(cadeira).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CadeiraExists(id))
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

        // DELETE: api/Cadeiras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCadeira(int id)
        {
            var cadeira = await _context.Cadeiras.FindAsync(id);
            if (cadeira == null)
            {
                return NotFound();
            }

            _context.Cadeiras.Remove(cadeira);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CadeiraExists(int id)
        {
            return _context.Cadeiras.Any(e => e.Id == id);
        }
    }
}