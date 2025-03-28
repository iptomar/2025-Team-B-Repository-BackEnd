using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstituicaoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InstituicaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Instituicao> data = await _context.Instituicao.ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Select_Ind(string id)
        {
            Instituicao registo = await _context.Instituicao.FindAsync(id);
            return Ok(registo);
        }

        [HttpPost("instituicao")]
        public async Task<IActionResult> Insert([FromBody] Instituicao instituicao)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            await _context.Instituicao.AddAsync(instituicao);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = instituicao.id_instituicao }, instituicao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Instituicao instituicao)
        {
            _context.Instituicao.Update(instituicao);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Instituicao registo = await _context.Instituicao.FindAsync(id);
            _context.Instituicao.Remove(registo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
