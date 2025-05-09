using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
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
         * Estado: ✓
         */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Sala> data = await _context.Sala.ToListAsync();
            return Ok(data);
        }

        /**
         * Endpoint destinado à Seleção de Salas segundo a Localidade
         * Estado: ✓
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
         * Endpoint de Inserção das Salas
         * Estado: ✓
         * 
         * @param Localidade - Nome da Localidade da Sala
         */
        [HttpPost("{Localidade}")]
        public async Task<IActionResult> Insert([FromBody] Sala sala, string Localidade)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Encontro do ID da Localidade 
            var localObj = await (from local in _context.Localidade
                                  where local.Nome_localidade == Localidade
                                  select new
                                  {
                                      Id_Localidade = local.Id_localidade
                                  }).FirstOrDefaultAsync();
            // Caso em que foi encontrada a localidade definida
            if (localObj != null)
            {
                sala.Localidade = localObj.Id_Localidade;
                await _context.Sala.AddAsync(sala);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = sala.Id_sala }, sala);
            }
            // Caso em que não foi encontrada a localidade definida
            return NotFound("A localidade que especificou não existe na BD.");
        }

        /**
         * Endpoint de Edição das Salas
         * Estado: ✓
         * 
         * @param Localidade - Nome da Localidade da Sala
         * @param Nome_Sala - Nome da Sala
         */
        [HttpPut("{Localidade}/{Nome_Sala}")]
        public async Task<IActionResult> Edit([FromBody] Sala sala, string Localidade, string Nome_Sala)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do ID da Localidade 
            var salaObj = await (from local in _context.Localidade
                                 join sal in _context.Sala on local.Id_localidade equals sal.Localidade
                                 where local.Nome_localidade == Localidade
                                 select new
                                 {
                                     Id_Localidade = local.Id_localidade
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrada a localidade definida
            if (salaObj != null)
            {
                sala.Nome_sala = Nome_Sala;
                sala.Localidade = salaObj.Id_Localidade;
                _context.Sala.Update(sala);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrada a localidade definida
            return NotFound("A localidade que especificou não existe na BD.");
        }

        /**
         * Endpoint de Apagamento das Salas
         * 
         * @param Nome_Sala - Nome da Sala
         */
        [HttpDelete("{Nome_Sala}")]
        public async Task<IActionResult> Delete(string Nome_sala)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do ID da Sala 
            var salaObj = await (from sal in _context.Sala
                                 where sal.Nome_sala == Nome_sala
                                 select new
                                 {
                                     Id_Localidade = sal.Id_sala
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrada a sala a apagar
            if (salaObj != null)
            {
                // Encontra a sala pelo o seu nome
                Sala sala = await _context.Sala.FindAsync(salaObj.Id_Localidade);
                // Apaga a mesma e salva as alterações na BD
                _context.Sala.Remove(sala);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrada a sala a apagar
            return NotFound("A localidade que especificou não existe na BD.");
        }

    }
}
