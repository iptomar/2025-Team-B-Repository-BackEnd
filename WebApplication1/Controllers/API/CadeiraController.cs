using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadeiraController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CadeiraController (ApplicationDbContext context)
        {
            _context = context;
        }

        /**
        * Endpoint da Lista de Cadeiras
        */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Cadeira> data = await _context.Cadeira.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint para pesquisar uma cadeira
        * @param Nome - Nome do Cadeira
        */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Ind(string Nome)
        {
            Cadeira registo = await _context.Cadeira.FindAsync(Nome);
            return Ok(registo);
        }

        /**
        * Endpoint para inserir uma cadeira
        */
        [HttpPost("cadeira")]
        public async Task<IActionResult> Insert([FromBody] Cadeira cadeira)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            await _context.Cadeira.AddAsync(cadeira);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = cadeira.Id_cadeira }, cadeira);
        }

        /**
        * Endpoint para atualizar uma cadeira
        * @param Id - Id do Cadeira
        */
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Cadeira cadeira)
        {
            _context.Cadeira.Update(cadeira);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
        * Endpoint para eliminar uma cadeira
        * @param Id - Id do Cadeira
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Cadeira registo = await _context.Cadeira.FindAsync(id);
            _context.Cadeira.Remove(registo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
