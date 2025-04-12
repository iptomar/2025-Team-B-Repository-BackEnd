using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    public class SalaController : ControllerBase
    {
        // Referência à BD
        private readonly ApplicationDbContext _context;

        /*
         * Construtor Parametrizado
         * 
         * @param context - Contexto da BD
         */
        public SalaController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Endpoint de Seleção Genérica das Salas
         */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Sala> data = await _context.Sala.ToListAsync();
            return Ok(data);
        }

        /**
         * Endpoint destinado à Seleção de Salas segundo a Localidade
         * 
         * @param Localidade - Nome da Localidade da Sala
         */
        [HttpGet("{Localidade}")]
        public async Task<IActionResult> Select_Sala(string Localidade)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Fetch da(s) Sala(s) segundo a Localidade 
            var registo = await (from sala in _context.Sala
                                 join local in _context.Localidade on sala.Localidade equals local.Id_localidade
                                 where local.Nome_localidade == Localidade
                                 select new
                                 {
                                     Sala = sala.Nome_sala
                                 }).ToListAsync();
            // Caso em que não foi encontrada nenhuma sala segundo os parâmetros recebidos
            if (registo == null || !registo.Any())
            {
                return NotFound("Nenhuma sala encontrada para o critério fornecido.");
            }
            // Caso em que foi encontrada pelo menos uma turma segundo os parâmetros recebidos 
            return Ok(registo);
        }

        /**
         * Endpoint de Inserção Genérica das Salas
         */
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Sala sala)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Sala.AddAsync(sala);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = sala.Id_sala }, sala);
        }

        /**
         * Endpoint de Edição Genérica das Salas
         */
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Sala sala)
        {
            _context.Sala.Update(sala);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
         * Endpoint de Apagamento Génerico das Salas
         */
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            Sala sala = await _context.Sala.FindAsync();
            _context.Sala.Remove(sala);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
