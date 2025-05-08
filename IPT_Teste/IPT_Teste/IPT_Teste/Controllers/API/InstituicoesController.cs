using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Models;
using System;
using IPT_Teste.Data;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstituicoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InstituicoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Instituicoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instituicoes>>> GetInstituicoes()
        {
            return await _context.Instituicoes
                                 .Include(i => i.Localidade)
                                 .ToListAsync();
        }

        // GET: api/Instituicoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Instituicoes>> GetInstituicao(int id)
        {
            var instituicao = await _context.Instituicoes
                                            .Include(i => i.Localidade)
                                            .FirstOrDefaultAsync(i => i.Id == id);

            if (instituicao == null)
            {
                return NotFound();
            }

            return instituicao;
        }

        // POST: api/Instituicoes
        [HttpPost]
        public async Task<ActionResult<Instituicoes>> PostInstituicao(Instituicoes instituicao)
        {
            // Verifica se a Localidade existe
            var localidade = await _context.Localidades.FindAsync(instituicao.LocalidadeFK);
            if (localidade == null)
            {
                return BadRequest("Localidade não encontrada.");
            }

            _context.Instituicoes.Add(instituicao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInstituicao), new { id = instituicao.Id }, instituicao);
        }

        // PUT: api/Instituicoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstituicao(int id, Instituicoes instituicao)
        {
            if (id != instituicao.Id)
            {
                return BadRequest();
            }

            // Confirma se a localidade referenciada existe
            if (!_context.Localidades.Any(l => l.Id == instituicao.LocalidadeFK))
            {
                return BadRequest("Localidade não encontrada.");
            }

            _context.Entry(instituicao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstituicaoExists(id))
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

        // DELETE: api/Instituicoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstituicao(int id)
        {
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null)
            {
                return NotFound();
            }

            _context.Instituicoes.Remove(instituicao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InstituicaoExists(int id)
        {
            return _context.Instituicoes.Any(e => e.Id == id);
        }
    }
}
