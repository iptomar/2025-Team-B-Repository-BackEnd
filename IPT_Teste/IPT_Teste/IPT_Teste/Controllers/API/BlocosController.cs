using IPT_Teste.Data;
using IPT_Teste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocosController : ControllerBase
    {
        // Referência à BD 
        private readonly ApplicationDbContext _context;

        /*
         * Construtor Parametrizado
         * 
         * @param context - Contexto da BD
         */
        public BlocosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Blocos
        /*
         * Endpoint de Seleção Genérica dos Blocos
         * Estado: ✓
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blocos>>> GetBlocos()
        {
            return await _context.Blocos.Include(b => b.Sala).Include(b => b.Aula).ToListAsync();
        }

        // GET: api/Blocos/5
        /**
        * Endpoint destinado à Seleção de Blocos segundo o seu ID
        * Estado: ✓
        * 
        * @param id - ID do Bloco
        */
        [HttpGet("{id}")]
        public async Task<ActionResult<Blocos>> GetBloco(int id)
        {
            // Pesquisa do Bloco segundo o seu ID
            var bloco = await _context.Blocos
                .Include(b => b.Sala)
                .Include(b => b.Aula)
                .FirstOrDefaultAsync(b => b.Id == id);
            // Caso em que não foi encontrado nenhum Bloco segundo o parâmetro recebido
            if (bloco == null)
            {
                return NotFound();
            }
            // Caso em que foi encontrado pelo menos um Bloco segundo o parâmetro recebido
            return bloco;
        }

        // POST: api/Blocos
        /**
         * Endpoint de Inserção de Blocos
         * Estado: ✓
         * 
         * @param bloco - Objeto do Tipo "Blocos" a ser inserido
         */
        [HttpPost]
        public async Task<ActionResult<Blocos>> PostBloco(Blocos bloco)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Tenho dúvidas na implementação de não colisões
            // Caso em que não foi encontrado nenhum Ano Letivo com o mesmo Ano
            _context.Blocos.Add(bloco);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBloco), new { id = bloco.Id }, bloco);
        }

        // PUT: api/Blocos/5
        /**
         * Endpoint para Atualizar os Blocos
         * Estado: ✓
         * 
         * @param id - ID do Bloco
         * @param bloco - Objeto do tipo "Blocos" a ser atualizado
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBloco(int id, Blocos bloco)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o ID fornecido é distinto do Bloco fornecido
            if (id != bloco.Id)
            {
                return BadRequest();
            }
            // Atualização dos estado da entidade a ser atualizada
            _context.Entry(bloco).State = EntityState.Modified;
            // Atualização realizada com sucesso
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Blocos/5
        /**
         * Endpoint de Apagamento dos Blocos
         * Estado: ✓
         * 
         * @param id - ID do Bloco
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBloco(int id)
        {
            // Pesquisa do Bloco pelo seu ID
            var bloco = await _context.Blocos.FindAsync(id);
            // Caso em não foi encontrado o Bloco pretendido
            if (bloco == null)
            {
                return NotFound();
            }
            // Apaga o mesmo e salva as alterações na BD
            _context.Blocos.Remove(bloco);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
