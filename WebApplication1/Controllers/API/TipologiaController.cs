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

        /**
        * Endpoint da Lista das Tipologias
        */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Tipologia> data = await _context.Tipologia.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint para pesquisar uma tipologia
        * @param Nome - Nome da Tipologia
        */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Ind(string Nome)
        {
            Tipologia registo = await _context.Tipologia.FindAsync(Nome);
            return Ok(registo);
        }

        /**
        * Endpoint para inserir uma tipologia
        */
        [HttpPost("tipologia")]
        public async Task<IActionResult> Insert([FromBody] Tipologia tipologia)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }

            await _context.Tipologia.AddAsync(tipologia);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = tipologia.Id_tipologia }, tipologia);
        }

        /**
        * Endpoint para atualizar uma tipologia
        * @param Id - Id da Tipologia
        */
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Tipologia tipologia)
        {
            _context.Tipologia.Update(tipologia);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
        * Endpoint para eliminar uma tipologia
        * @param Id - Id da Tipologia
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Tipologia registo = await _context.Tipologia.FindAsync(id);
            _context.Tipologia.Remove(registo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
         * Endpoint destinado à Selecao do Nome da Tipologia
         */
        [HttpGet("SelectNomeTipologia")]
        public async Task<IActionResult> SelectNomeTipologia()
        {
            /**
             * Funcionamento:
             *  - Seleciona o nome da tipologia
             *  - Através da Tabela Tipologia
             */
            var nome = _context.Tipologia.
                Select(t => t.Nome_tipologia)
                .ToListAsync();

            return Ok(nome);
        }
    }
}
