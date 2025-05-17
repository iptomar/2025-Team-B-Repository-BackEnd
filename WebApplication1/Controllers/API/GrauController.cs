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
        // Referência à BD
        private readonly ApplicationDbContext _context;

        /*
        * Construtor Parametrizado
        * 
        * @param context - Contexto da BD
        */
        public GrauController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Endpoint de Seleção Genérica das Graus
         * Estado: ✓
         */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Grau> data = await _context.Grau.ToListAsync();
            return Ok(data);
        }

        /**
         * Endpoint destinado à Seleção de Graus segundo o seu Nome
         * Estado: ✓
         * 
         * @param Nome_grau - Nome do Grau
         */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Gra(string Nome_grau)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do Grau segundo o seu Nome 
            var registo = await (from grau in _context.Grau
                                 where grau.Nome_grau == Nome_grau
                                 select new
                                 {
                                     Id = grau.Id_grau,
                                     Grau = grau.Nome_grau
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrada nenhum Grau segundo os parâmetros recebidos
            if (registo == null)
            {
                return NotFound("Nenhum grau encontrado para o critério fornecido.");
            }
            // Caso em que foi encontrado pelo menos um Grau segundo os parâmetros recebidos 
            return Ok(registo);
        }

        /**
        * Endpoint de Inserção de Graus
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Grau grau)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o Grau a ser inserido já existe
            var registo = await (from gra in _context.Grau
                                 where gra.Nome_grau == grau.Nome_grau
                                 select new
                                 {
                                     Id = gra.Id_grau
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrado nenhum Grau com o mesmo nome
            if(registo == null)
            {
                await _context.Grau.AddAsync(grau);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = grau.Id_grau }, grau);
            }
            // Caso em que foi encontrado pelo menos um Grau com o mesmo nome
            return NotFound("O Grau que deseja inserir já se encontra na BD.");
        }

        /**
        * Endpoint para Atualizar os Graus
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Grau grau)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do Grau pelo seu ID 
            var registo = await (from gr in _context.Grau
                                 where gr.Id_grau == grau.Id_grau
                                 select new
                                 {
                                     Id_grau = gr.Id_grau
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrado o grau definido
            if (registo != null)
            {
                _context.Grau.Update(grau);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrado o grau definido
            return NotFound("O grau que especificou não existe na BD.");
        }

        /**
         * Endpoint de Apagamento dos Graus
         * Estado: ?
         * 
         * @param Nome_grau - Nome do Grau
         */
        [HttpDelete("{Nome_grau}")]
        public async Task<IActionResult> Delete(string Nome_grau)
        {
            // Pesquisa do Grau pelo seu Nome 
            var registo = await (from gr in _context.Grau
                                 where gr.Nome_grau == Nome_grau
                                 select new
                                 {
                                     Id_grau = gr.Id_grau
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrado o Grau definido
            if (registo != null)
            {
                // Encontra o Grau pelo o seu Nome
                Grau grau = await _context.Grau.FindAsync(registo.Id_grau);
                // Apaga o mesmo e salva as alterações na BD
                _context.Grau.Remove(grau);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrado o grau definido
            return NotFound("O grau que especificou não existe na BD.");
        }
    }
}
