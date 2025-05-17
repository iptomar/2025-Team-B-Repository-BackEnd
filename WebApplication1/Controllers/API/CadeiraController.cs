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
        * Estado: ✓
        */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Cadeira> data = await _context.Cadeira.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint para pesquisar uma cadeira
        * Estado: ✓
        * 
        * @param Nome - Nome do Cadeira
        */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Cad(string Nome)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do Professor segundo o Nome 
            var registo = await (from cad in _context.Cadeira
                                 where cad.Nome_cadeira == Nome
                                 select new
                                 {
                                     Id = cad.Id_cadeira,
                                     Cadeira = cad.Nome_cadeira
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrada nenhum cadeira segundo os parâmetros recebidos
            if (registo == null)
            {
                return NotFound("Nenhuma cadeira encontrado para o critério fornecido.");
            }
            // Caso em que foi encontrado pelo menos um cadeira segundo os parâmetros recebidos 
            return Ok(registo);
        }

        /**
        * Endpoint para inserir uma cadeira
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPost("cadeira")]
        public async Task<IActionResult> Insert([FromBody] Cadeira cadeira)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se a Cadeira a ser inserido já existe
            var registo = await (from cad in _context.Cadeira
                                 where cad.Nome_cadeira == cadeira.Nome_cadeira && cad.ECTS == cadeira.ECTS
                                 select new
                                 {
                                     Id = cad.Id_cadeira
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrado nenhuma Cadeira com o mesmo nome
            if (registo == null)
            {
                await _context.Cadeira.AddAsync(cadeira);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = cadeira.Id_cadeira }, cadeira);
            }
            // Caso em que foi encontrado pelo menos uma cadeira com o mesmo nome
            return NotFound("A cadeira que deseja inserir já se encontra na BD.");
        }

        /**
        * Endpoint para atualizar uma cadeira
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Cadeira cadeira)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do Professor pelo seu ID 
            var registo = await (from cad in _context.Cadeira
                                 where cad.Id_cadeira == cadeira.Id_cadeira
                                 select new
                                 {
                                     Id_cadeira = cad.Id_cadeira
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrado o professor definido
            if (registo != null)
            {
                _context.Cadeira.Update(cadeira);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrado o cadeira definido
            return NotFound("A cadeira que especificou não existe na BD.");
        }

        /**
        * Endpoint para eliminar uma cadeira
        * Estado: ✓
        * 
        * @param Nome - Nome da Cadeira
        */
        [HttpDelete("{Nome}")]
        public async Task<IActionResult> Delete(string Nome)
        {
            // Pesquisa do Cadeira pelo seu Nome 
            var registo = await (from cad in _context.Cadeira
                                 where cad.Nome_cadeira == Nome
                                 select new
                                 {
                                     Id_cadeira = cad.Id_cadeira
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrado o Cadeira definido
            if (registo != null)
            {
                // Encontra o Cadeira pelo o seu Nome
                Cadeira cad = await _context.Cadeira.FindAsync(registo.Id_cadeira);
                // Apaga o mesmo e salva as alterações na BD
                _context.Cadeira.Remove(cad);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrado o cadeira definido
            return NotFound("A cadeira que especificou não existe na BD.");
        }
    }
}
