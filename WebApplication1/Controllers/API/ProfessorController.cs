using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        /**
        * Endpoint da Lista de Professores
        */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Professor> data = await _context.Professor.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint para pesquisar um professor
        * @param Nome - Nome do Professor
        */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Ind(string Nome)
        {
            Professor registo = await _context.Professor.FindAsync(Nome);
            return Ok(registo);
        }

        /**
        * Endpoint para inserir um professor
        */
        [HttpPost("professor")]
        public async Task<IActionResult> Insert([FromBody] Professor professor)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            await _context.Professor.AddAsync(professor);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = professor.Id_professor }, professor);
        }

        /**
        * Endpoint para atualizar um professor
        * @param Id - Id do Professor
        */
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Professor professor)
        {
            _context.Professor.Update(professor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
        * Endpoint para eliminar um professor
        * @param Id - Id do Professor
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Professor registo = await _context.Professor.FindAsync(id);
            _context.Professor.Remove(registo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
