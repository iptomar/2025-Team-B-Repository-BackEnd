using Microsoft.AspNetCore.Mvc;
using IPT_Teste.Models;
using Microsoft.EntityFrameworkCore;
using System;
using IPT_Teste.Data;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalidadesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocalidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Localidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Localidades>>> GetLocalidades()
        {
            return await _context.Localidades.ToListAsync();
        }

        // GET: api/Localidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Localidades>> GetLocalidade(int id)
        {
            var localidade = await _context.Localidades.FindAsync(id);

            if (localidade == null)
            {
                return NotFound();
            }

            return localidade;
        }

        // POST: api/Localidades
        [HttpPost]
        public async Task<ActionResult<Localidades>> PostLocalidade(Localidades localidade)
        {
            _context.Localidades.Add(localidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocalidade), new { id = localidade.Id }, localidade);
        }

        // PUT: api/Localidades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocalidade(int id, Localidades localidade)
        {
            if (id != localidade.Id)
            {
                return BadRequest();
            }

            _context.Entry(localidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalidadeExists(id))
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

        // DELETE: api/Localidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocalidade(int id)
        {
            var localidade = await _context.Localidades.FindAsync(id);
            if (localidade == null)
            {
                return NotFound();
            }

            _context.Localidades.Remove(localidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocalidadeExists(int id)
        {
            return _context.Localidades.Any(e => e.Id == id);
        }
    }
}