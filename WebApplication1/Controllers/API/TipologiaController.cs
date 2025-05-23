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
        // Referência à BD
        private readonly ApplicationDbContext _context;

        /*
        * Construtor Parametrizado
        * 
        * @param context - Contexto da BD
        */
        public TipologiaController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Endpoint de Seleção Genérica das Tipologias
         * Estado: ✓
         */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Tipologia> data = await _context.Tipologia.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint destinado à Seleção da Tipologia segundo o seu Nome
        * Estado: ✓
        * 
        * @param Nome_tipologia - Nome da Tipologia
        */
        [HttpGet("{Nome_tipologia}")]
        public async Task<IActionResult> Select_Tip(string Nome_tipologia)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa da Tipologia segundo o Nome 
            var registo = await (from tip in _context.Tipologia
                                 where tip.Nome_tipologia == Nome_tipologia
                                 select new
                                 {
                                     Id_tipologia = tip.Id_tipologia,
                                     Nome_tipologia = tip.Nome_tipologia
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrada nenhuma tipologia segundo os parâmetros recebidos
            if (registo == null)
            {
                return NotFound("Nenhuma tipologia encontrada para o critério fornecido.");
            }
            // Caso em que foi encontrado pelo menos uma tipologia segundo os parâmetros recebidos 
            return Ok(registo);
        }

        /**
        * Endpoint de Inserção de Tipologias
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPost("tipologia")]
        public async Task<IActionResult> Insert([FromBody] Tipologia tipologia)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState); 
            }
            // Verificação se a Tipologia a ser inserida já existe
            var registo = await (from tip in _context.Tipologia
                                 where tip.Nome_tipologia == tipologia.Nome_tipologia
                                 select new
                                 {
                                     Id = tip.Id_tipologia
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrado nenhuma Tipologia com o mesmo nome
            if (registo == null)
            {
                await _context.Tipologia.AddAsync(tipologia);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = tipologia.Id_tipologia }, tipologia);
            }
            // Caso em que foi encontrado pelo menos um Grau com o mesmo nome
            return NotFound("A Tipologia que deseja inserir já se encontra na BD.");
        }

        /**
        * Endpoint para Atualizar as Tipologias
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Tipologia tipologia)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa da Tipologia pelo seu ID
            var registo = await (from tip in _context.Tipologia
                                 where tip.Id_tipologia == tipologia.Id_tipologia
                                 select new
                                 {
                                     Id_tipologia = tip.Id_tipologia
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrada a tipologia definida
            if (registo != null)
            {
                _context.Tipologia.Update(tipologia);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrada a tipologia definida
            return NotFound("A Tipologia que especificou não existe na BD.");
        }

        /**
         * Endpoint de Apagamento de Tipologias
         * Estado: ✓
         * 
         * @param Nome_tipologia - Nome da Tipologia
         */
        [HttpDelete("{Nome_tipologia}")]
        public async Task<IActionResult> Delete(string Nome_tipologia)
        {
            // Pesquisa da Tipologia pelo seu Nome 
            var registo = await (from tip in _context.Tipologia
                                 where tip.Nome_tipologia == Nome_tipologia
                                 select new
                                 {
                                     Id_tipologia = tip.Id_tipologia
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrada a Tipologia definida
            if (registo != null)
            {
                // Encontra a Tipologia pelo o seu ID
                Tipologia tipologia = await _context.Tipologia.FindAsync(registo.Id_tipologia);
                // Apaga a mesma e salva as alterações na BD
                _context.Tipologia.Remove(tipologia);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrada a tipologia definida
            return NotFound("A tipologia que especificou não existe na BD.");
        }

        /**
         * Endpoint destinado à Selecao do Nome da Tipologia
         */
        /*[HttpGet("SelectNomeTipologia")]
        public async Task<IActionResult> SelectNomeTipologia()
        {
            var nome = _context.Tipologia.
                Select(t => t.Nome_tipologia)
                .ToListAsync();

            return Ok(nome);
        }*/
    }
}
