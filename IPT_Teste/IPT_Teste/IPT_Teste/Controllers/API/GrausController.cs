using IPT_Teste.Data;
using IPT_Teste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrausController : ControllerBase
    {
        // Referência à BD
        private readonly ApplicationDbContext _context;

        /*
         * Construtor Parametrizado
         * 
         * @param context - Contexto da BD
         */
        public GrausController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Endpoint de Seleção Genérica das Graus
         * Estado: ✓
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Graus>>> GetGraus()
        {
            return await _context.Graus.ToListAsync();
        }

        /**
         * Endpoint destinado à Seleção de Graus segundo o seu ID
         * Estado: ✓
         * 
         * @param id - ID do Grau
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<Graus>> GetGrau(int id)
        {
            // Pesquisa do Grau segundo o seu ID
            var grau = await _context.Graus.FindAsync(id);
            // Caso em que não foi encontrado nenhum Grau segundo o parâmetro recebido
            if (grau == null)
            {
                return NotFound();
            }
            // Caso em que foi encontrado pelo menos um Grau segundo o parâmetro recebido
            return grau;
        }

        /**
         * Endpoint de Inserção de Graus
         * Estado: ✓
         * 
         * @param grau - Objeto do Tipo "Grau" a ser inserido
         */
        [HttpPost]
        public async Task<ActionResult<Graus>> PostGrau(Graus grau)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o Grau a ser inserido já existe
            var registo = await (from gra in _context.Graus
                                 where gra.Grau == grau.Grau
                                 select new
                                 {
                                     Id = gra.Id
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrado nenhum Grau com o mesmo nome
            if (registo == null)
            {
                _context.Graus.Add(grau);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetGrau), new { id = grau.Id }, grau);
            }
            // Caso em que foi encontrado pelo menos um Grau com o mesmo nome
            return BadRequest("O Grau que deseja inserir já se encontra na BD.");
        }

        /**
         * Endpoint para Atualizar os Graus
         * Estado: ✓
         * 
         * @param id - ID do Curso
         * @param grau - Objeto do tipo "Grau" a ser atualizado
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrau(int id, Graus grau)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o ID fornecido é distinto do Grau fornecido
            if (id != grau.Id)
            {
                return BadRequest();
            }
            // Atualização dos estado da entidade a ser atualizada
            _context.Entry(grau).State = EntityState.Modified;
            try
            {
                // Tentativa da Atualização do Grau
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Caso em que não foi encontrado um Grau com o ID fornecido
                if (!GrauExists(id))
                {
                    return NotFound();
                }
                // Caso em que foi encontrado um Grau com o ID fornecido
                else
                {
                    throw;
                }
            }
            // Atualização realizada com sucesso
            return NoContent();
        }

        /**
         * Endpoint de Apagamento dos Graus
         * Estado: ✓
         * 
         * @param id - ID do Grau
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrau(int id)
        {
            // Pesquisa do Grau pelo seu ID
            var grau = await _context.Graus.FindAsync(id);
            // Caso em não foi encontrado o Grau pretendido
            if (grau == null)
            {
                return NotFound();
            }
            // Apaga o mesmo e salva as alterações na BD
            _context.Graus.Remove(grau);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
         * Método para Verificação da Existência de um Grau
         * Estado: ✓
         * 
         * @param id - ID do Grau
         */
        private bool GrauExists(int id)
        {
            return _context.Graus.Any(e => e.Id == id);
        }
    }
}