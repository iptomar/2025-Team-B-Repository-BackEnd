using IPT_Teste.Data;
using IPT_Teste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadeirasController : ControllerBase
    {
        // Referência à BD
        private readonly ApplicationDbContext _context;

        /*
        * Construtor Parametrizado
        * 
        * @param context - Contexto da BD
        */
        public CadeirasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cadeiras
        /*
         * Endpoint de Seleção Genérica das Cadeiras
         * Estado: ✓
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cadeiras>>> GetCadeiras()
        {
            return await _context.Cadeiras.ToListAsync();
        }

        // GET: api/Cadeiras/5
        /**
        * Endpoint destinado à Seleção de Cadeiras segundo o seu ID
        * Estado: ✓
        * 
        * @param id - ID da Cadeira
        */
        [HttpGet("{id}")]
        public async Task<ActionResult<Cadeiras>> GetCadeira(int id)
        {
            // Pesquisa da Cadeira segundo o seu ID
            var cadeira = await _context.Cadeiras.FindAsync(id);
            // Caso em que não foi encontrada nenhuma Cadeira segundo o parâmetro recebido
            if (cadeira == null)
            {
                return NotFound();
            }
            // Caso em que foi encontrada pelo menos uma Cadeira segundo o parâmetro recebido
            return cadeira;
        }

        // POST: api/Cadeiras
        /**
         * Endpoint de Inserção de Cadeiras
         * Estado: ✓
         * 
         * @param bloco - Objeto do Tipo "Cadeiras" a ser inserido
         */
        [HttpPost]
        public async Task<ActionResult<Cadeiras>> PostCadeira(Cadeiras cadeira)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se a Cadeira a ser inserida já existe
            var registo = await (from cad in _context.Cadeiras
                                 where cad.Cadeira == cadeira.Cadeira
                                 select new
                                 {
                                     Id = cad.Id
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrada nenhuma Cadeira
            if (registo == null)
            {
                _context.Cadeiras.Add(cadeira);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCadeira), new { id = cadeira.Id }, cadeira);
            }
            // Caso em que foi encontrado pelo menos uma Cadeira com o mesmo nome
            return BadRequest("A Cadeira que deseja inserir já se encontra na BD.");
        }

        // PUT: api/Cadeiras/5
        /**
         * Endpoint para Atualizar as Cadeiras
         * Estado: ✓
         * 
         * @param id - ID da Cadeira
         * @param grau - Objeto do tipo "Cadeiras" a ser atualizado
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCadeira(int id, Cadeiras cadeira)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o ID fornecido é distinto da Cadeira fornecida
            if (id != cadeira.Id)
            {
                return BadRequest();
            }
            // Atualização dos estado da entidade a ser atualizada
            _context.Entry(cadeira).State = EntityState.Modified;

            try
            {
                // Tentativa da Atualização da Cadeira
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Caso em que não foi encontrada uma Cadeira com o ID fornecido
                if (!CadeiraExists(id))
                {
                    return NotFound();
                }
                // Caso em que foi encontrado uma Cadeira com o ID fornecido
                else
                {
                    throw;
                }
            }
            // Atualização realizada com sucesso
            return NoContent();
        }

        // DELETE: api/Cadeiras/5
        /**
         * Endpoint de Apagamento das Cadeiras
         * Estado: ✓
         * 
         * @param id - ID da Cadeira
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCadeira(int id)
        {
            // Pesquisa do Ano Letivo pelo seu ID
            var cadeira = await _context.Cadeiras.FindAsync(id);
            // Caso em não foi encontrada a Cadeira pretendida
            if (cadeira == null)
            {
                return NotFound();
            }
            // Apaga a mesma e salva as alterações na BD
            _context.Cadeiras.Remove(cadeira);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
         * Método para Verificação da Existência de uma Cadeira
         * Estado: ✓
         * 
         * @param id - ID da Cadeira
         */
        private bool CadeiraExists(int id)
        {
            return _context.Cadeiras.Any(e => e.Id == id);
        }
    }
}