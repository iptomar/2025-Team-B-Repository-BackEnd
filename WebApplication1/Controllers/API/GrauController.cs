using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrauController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GrauController(ApplicationDbContext context)
        {
            _context = context;
        }

        /**
        * Endpoint da Lista de Graus
        */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Grau> data = await _context.Grau.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint de pesquisa de um curso
        * @param Nome - Nome do Curso
        */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Ind(string Nome)
        {
            Grau registo = await _context.Grau.FindAsync(Nome);
            return Ok(registo);
        }

        /**
        * Endpoint para inserir um grau
        */
        [HttpPost("grau")]
        public async Task<IActionResult> Insert([FromBody] Grau grau)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            await _context.Grau.AddAsync(grau);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = grau.Id_grau }, grau);
        }

        /**
        * Endpoint para atualizar um grau
        * @param Id - Id do Grau
        */
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Grau grau)
        {
            _context.Grau.Update(grau);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
        * Endpoint para eliminar um grau
        * @param Id - Id do Grau
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Grau registo = await _context.Grau.FindAsync(id);
            _context.Grau.Remove(registo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
        * Endpoint destinado à Selecao do Nome do Grau
        */
        [HttpGet("SelectNomeGrau")]
        public async Task<IActionResult> SelectNomeGrau()
        {
            /**
             * Funcionamento:
             *  - Seleciona o nome do grau
             *  - Através da Tabela Grau
             */
            var nome = _context.Grau.
                Select(g => g.Nome_grau)
                .ToListAsync();

            return Ok(nome);
        }
    }

}
