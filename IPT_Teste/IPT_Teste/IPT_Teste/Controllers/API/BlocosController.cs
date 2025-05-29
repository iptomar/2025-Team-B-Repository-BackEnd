using Microsoft.AspNetCore.Mvc;
using IPT_Teste.Models;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Data;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlocosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Blocos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blocos>>> GetBlocos()
        {
            return await _context.Blocos.Include(b => b.Sala).Include(b => b.Aula).ToListAsync();
        }

        // GET: api/Blocos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Blocos>> GetBloco(int id)
        {
            var bloco = await _context.Blocos
                .Include(b => b.Sala)
                .Include(b => b.Aula)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bloco == null)
            {
                return NotFound();
            }

            return bloco;
        }

        // POST: api/Blocos
        [HttpPost]
        public async Task<ActionResult<Blocos>> PostBloco(Blocos bloco)
        {
            _context.Blocos.Add(bloco);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBloco), new { id = bloco.Id }, bloco);
        }

        // PUT: api/Blocos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBloco(int id, Blocos bloco)
        {
            if (id != bloco.Id)
            {
                return BadRequest();
            }

            _context.Entry(bloco).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Blocos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBloco(int id)
        {
            var bloco = await _context.Blocos.FindAsync(id);
            if (bloco == null)
            {
                return NotFound();
            }

            _context.Blocos.Remove(bloco);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
