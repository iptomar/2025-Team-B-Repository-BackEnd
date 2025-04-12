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

        /**
        * Endpoint da Lista de Instituições
        */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Instituicao> data = await _context.Instituicao.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint para pesquisar uma instituição
        * @param Nome - Nome da Instituição
        */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Ind(string Nome)
        {
            Instituicao registo = await _context.Instituicao.FindAsync(Nome);
            return Ok(registo);
        }

        /**
        * Endpoint para inserir uma instituição
        */
        [HttpPost("instituicao")]
        public async Task<IActionResult> Insert([FromBody] Instituicao instituicao)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            await _context.Instituicao.AddAsync(instituicao);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = instituicao.Id_instituicao }, instituicao);
        }

        /**
        * Endpoint para atualizar uma instituição
        * @param Id - Id da Instituição
        */
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Instituicao instituicao)
        {
            _context.Instituicao.Update(instituicao);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
        * Endpoint para eliminar uma instituição
        * @param Id - Id da Instituição
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Instituicao registo = await _context.Instituicao.FindAsync(id);
            _context.Instituicao.Remove(registo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
         * Endpoint destinado à Selecao do Nome da Instituição
         */
        [HttpGet("SelectNomeInstituicao")]
        public async Task<IActionResult> SelectNomeInstituicao()
        {
            /**
             * Funcionamento:
             *  - Seleciona o nome da instituição
             *  - Através da Tabela Instituição
             */
            var nome = _context.Instituicao.
                Select(i => i.Nome_instituicao)
                .ToListAsync();

            return Ok(nome);
        }

    }
}
