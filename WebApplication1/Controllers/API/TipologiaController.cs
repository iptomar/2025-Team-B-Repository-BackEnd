using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipologiaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TipologiaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Tipologia> data = await _context.Tipologia.ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Select_Ind(string id)
        {
            Tipologia registo = await _context.Tipologia.FindAsync(id);
            return Ok(registo);
        }

        [HttpPost("curso")]
        public async Task<IActionResult> Insert([FromBody] Tipologia tipologia)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }

            await _context.Tipologia.AddAsync(tipologia);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = tipologia.Id_tipologia }, tipologia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Tipologia tipologia)
        {
            _context.Tipologia.Update(tipologia);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Tipologia registo = await _context.Tipologia.FindAsync(id);
            _context.Tipologia.Remove(registo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> SelectNomeTipologia()
        {
            var nome = _context.Tipologia.
                Select(t => t.Nome_tipologia)
                .ToListAsync();

            return Ok(nome);
        }
    }
}
