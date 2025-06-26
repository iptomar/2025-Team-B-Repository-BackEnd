using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CursoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Curso> data = await _context.Curso.ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Select_Ind(string id)
        {
            Curso registo = await _context.Curso.FindAsync(id);
            return Ok(registo);
        }

        [HttpPost("curso")]
        public async Task<IActionResult> Insert([FromBody] Curso curso)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            await _context.Curso.AddAsync(curso);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = curso.Id_curso }, curso);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Curso curso)
        {
            _context.Curso.Update(curso);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Curso registo = await _context.Curso.FindAsync(id);
            _context.Curso.Remove(registo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
