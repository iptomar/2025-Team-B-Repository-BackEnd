using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Models;
using System;
using IPT_Teste.Data;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SalasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Salas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salas>>> GetSalas()
        {
            return await _context.Salas
                                 .Include(i => i.Localidade)
                                 .ToListAsync();
        }

        // GET: api/Salas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Salas>> GetSala(int id)
        {
            var sala = await _context.Salas
                                            .Include(i => i.Localidade)
                                            .FirstOrDefaultAsync(i => i.Id == id);

            if (sala == null)
            {
                return NotFound();
            }

            return sala;
        }

        // POST: api/Salas
        [HttpPost]
        public async Task<ActionResult<Salas>> PostSala(Salas sala)
        {
            // Verifica se a Localidade existe
            var localidade = await _context.Localidades.FindAsync(sala.LocalidadeFK);
            if (localidade == null)
            {
                return BadRequest("Localidade não encontrada.");
            }

            _context.Salas.Add(sala);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSala), new { id = sala.Id }, sala);
        }

        // PUT: api/Salas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSala(int id, Salas sala)
        {
            if (id != sala.Id)
            {
                return BadRequest();
            }

            // Confirma se a localidade referenciada existe
            if (!_context.Localidades.Any(l => l.Id == sala.LocalidadeFK))
            {
                return BadRequest("Localidade não encontrada.");
            }

            _context.Entry(sala).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaExists(id))
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

        // DELETE: api/Salas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSala(int id)
        {
            var sala = await _context.Salas.FindAsync(id);
            if (sala == null)
            {
                return NotFound();
            }

            _context.Salas.Remove(sala);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalaExists(int id)
        {
            return _context.Salas.Any(e => e.Id == id);
        }
    }
}
