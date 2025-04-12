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

        /**
         * Endpoint da Lista de Cursos
         */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Curso> data = await _context.Curso.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint de pesquisa de um curso
        * @param Nome - Nome do Curso
        */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Ind(string Nome)
        {
            Curso registo = await _context.Curso.FindAsync(Nome);
            return Ok(registo);
        }

        /**
        * Endpoint para inserir um novo curso
        */
        [HttpPost("curso")]
        public async Task<IActionResult> Insert([FromBody] Curso curso)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            await _context.Curso.AddAsync(curso);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = curso.Id_curso }, curso);
        }

        /**
        * Endpoint para atualizar um curso
        * @param Id - Id do Curso
        */
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Curso curso)
        {
            _context.Curso.Update(curso);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
        * Endpoint para eliminar um curso
        * @param Id - Id do Curso
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Curso registo = await _context.Curso.FindAsync(id);
            _context.Curso.Remove(registo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
         * Endpoint destinado à Selecao do Ano do Curso
         */
        [HttpGet("SelectAnoCurso")]
        public async Task<IActionResult> SelectAnoCurso()
        {
            /**
             * Funcionamento:
             *  - Seleciona o ano do curso
             *  - Através da Tabela Curso
             */
            var ano = _context.Curso.
                Select(c => c.Ano)
                .ToListAsync();

            return Ok(ano);
        }
    }
}
