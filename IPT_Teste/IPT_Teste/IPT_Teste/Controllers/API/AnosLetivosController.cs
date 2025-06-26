using IPT_Teste.Data;
using IPT_Teste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnosLetivosController : ControllerBase
    {
        // Referência à BD
        private readonly ApplicationDbContext _context;

        /*
         * Construtor Parametrizado
         * 
         * @param context - Contexto da BD
         */
        public AnosLetivosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AnoLetivos
        /*
         * Endpoint de Seleção Genérica dos Anos Letivos
         * Estado: ✓
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnosLetivos>>> GetAnoLetivos()
        {
            return await _context.AnosLetivos.ToListAsync();
        }

        // GET: api/AnoLetivos/5
        /**
         * Endpoint destinado à Seleção de Anos Letivos segundo o seu ID
         * Estado: ✓
         * 
         * @param id - ID do Ano Letivo
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<AnosLetivos>> GetAnoLetivo(int id)
        {
            // Pesquisa do Ano Letivo segundo o seu ID
            var anoLetivo = await _context.AnosLetivos.FindAsync(id);
            // Caso em que não foi encontrada nenhum Ano Letivo segundo o parâmetro recebido
            if (anoLetivo == null)
            {
                return NotFound();
            }
            // Caso em que foi encontrado pelo menos um Ano Letivo segundo o parâmetro recebido
            return anoLetivo;
        }

        // POST: api/AnoLetivos
        /**
         * Endpoint de Inserção de Anos Letivos
         * Estado: ✓
         * 
         * @param anoLetivo - Objeto do tipo "AnoLetivos" a ser inserido
         */
        [HttpPost]
        public async Task<ActionResult<AnosLetivos>> PostAnoLetivo(AnosLetivos anoLetivo)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o Ano Letivo a ser inserido já existe
            var registo = await (from ano in _context.AnosLetivos
                                 where ano.AnoLetivo == anoLetivo.AnoLetivo
                                 select new
                                 {
                                     Id = ano.Id
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrado nenhum Ano Letivo com o mesmo Ano
            if (registo == null)
            {
                _context.AnosLetivos.Add(anoLetivo);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAnoLetivo), new { id = anoLetivo.Id }, anoLetivo);
            }
            // Caso em que foi encontrado pelo menos um Ano Letivo com o mesmo nome
            return BadRequest("O Ano Letivo que deseja inserir já se encontra na BD.");
        }

        // PUT: api/AnoLetivos/5
        /**
         * Endpoint para Atualizar os Anos Letivos
         * Estado: ✓
         * 
         * @param id - ID do Ano Letivo
         * @param grau - Objeto do tipo "AnoLetivos" a ser atualizado
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnoLetivo(int id, AnosLetivos anoLetivo)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o ID fornecido é distinto do Ano Letivo fornecido
            if (id != anoLetivo.Id)
            {
                return BadRequest();
            }
            // Atualização dos estado da entidade a ser atualizada
            _context.Entry(anoLetivo).State = EntityState.Modified;
            try
            {
                // Tentativa da Atualização do Ano Letivo
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Caso em que não foi encontrado um Ano Letivo com o ID fornecido
                if (!AnoLetivoExists(id))
                {
                    return NotFound();
                }
                // Caso em que foi encontrado um Ano Letivo com o ID fornecido
                else
                {
                    throw;
                }
            }
            // Atualização realizada com sucesso
            return NoContent();
        }

        // DELETE: api/AnoLetivos/5
        /**
         * Endpoint de Apagamento dos Anos Letivos
         * Estado: ✓
         * 
         * @param id - ID do Ano Letivo
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnoLetivo(int id)
        {
            // Pesquisa do Ano Letivo pelo seu ID
            var anoLetivo = await _context.AnosLetivos.FindAsync(id);
            // Caso em não foi encontrado o Ano Letivo pretendido
            if (anoLetivo == null)
            {
                return NotFound();
            }
            // Apaga o mesmo e salva as alterações na BD
            _context.AnosLetivos.Remove(anoLetivo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
         * Método para Verificação da Existência de um Ano Letivo
         * Estado: ✓
         * 
         * @param id - ID do Ano Letivo
         */
        private bool AnoLetivoExists(int id)
        {
            return _context.AnosLetivos.Any(e => e.Id == id);
        }
    }
}