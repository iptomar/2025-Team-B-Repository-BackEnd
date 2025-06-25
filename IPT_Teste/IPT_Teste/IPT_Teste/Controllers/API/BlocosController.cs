using IPT_Teste.Data;
using IPT_Teste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Data;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Controllers.API;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocosController : ControllerBase
    {
        // Referência à BD 
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<BlocoHubController> _hubContext;

        public BlocosController(IHubContext<BlocoHubController> hubContext, ApplicationDbContext context)
        {
            _context = context;
            _hubContext = hubContext;
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


        // GET: api/Blocos/Turma/5
        [HttpGet("Turma/{id}")]
        public async Task<ActionResult<Blocos>> GetBlocoPorTurma(int id)
        {
            var blocos = await _context.Blocos
                .Include(b => b.Sala)
                .Include(b => b.Aula)
                .Where(b => b.Aula.TurmaFK == id)
                .ToListAsync();

            if (blocos == null)
            {
                return NotFound();
            }

            return Ok(blocos);

        }

        // GET: api/Blocos/Horario/5
        [HttpGet("Horario/{id}")]
        public async Task<ActionResult<Blocos>> GetBlocoPorHorario(int id)
        {

            var horario = await _context.Horarios
                .Include(h => h.Blocos)
                .Where(h => h.Id == id)
                .Select(h => h.Blocos)
                .ToListAsync();

            if (horario == null)
            {
                return NotFound();
            }

            return Ok(horario);

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

            var horarioId = await _context.Horarios.
                Include(h => h.Blocos).
                FirstOrDefaultAsync(h => h.Blocos.Any(b => b.Id == id));

            _context.Entry(bloco).State = EntityState.Modified;
            // Atualização realizada com sucesso
            await _context.SaveChangesAsync();
        
            await _hubContext.Clients.Group($"horario_{horarioId}")
                .SendAsync("UpdateBloco", bloco);

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
            
            await _hubContext.Clients.All.SendAsync("BlocoRemovido", new
            {
                BlocoId = id
            });

            return NoContent();
        }
    }
}
