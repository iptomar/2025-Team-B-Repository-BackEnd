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
    public class TipologiasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TipologiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Tipologias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tipologias>>> GetTipologias()
        {
            return await _context.Tipologias.ToListAsync();
        }

        // GET: api/Tipologias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tipologias>> GetTipologia(int id)
        {
            var tipologia = await _context.Tipologias.FindAsync(id);

            if (tipologia == null)
            {
                return NotFound();
            }

            return tipologia;
        }

        // POST: api/Tipologias
        [HttpPost]
        public async Task<ActionResult<Tipologias>> PostTipologia(Tipologias tipologia)
        {
            _context.Tipologias.Add(tipologia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTipologia), new { id = tipologia.Id }, tipologia);
        }

        // PUT: api/Tipologias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipologia(int id, Tipologias tipologia)
        {
            if (id != tipologia.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipologia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipologiaExists(id))
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

        // DELETE: api/Tipologias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipologia(int id)
        {
            var tipologia = await _context.Tipologias.FindAsync(id);
            if (tipologia == null)
            {
                return NotFound();
            }

            _context.Tipologias.Remove(tipologia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipologiaExists(int id)
        {
            return _context.Tipologias.Any(e => e.Id == id);
        }
    }
}