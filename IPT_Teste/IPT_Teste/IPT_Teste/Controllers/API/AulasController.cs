using IPT_Teste.Data;
using IPT_Teste.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AulasController : ControllerBase
    {
        // Referência à BD e Declaração do UserManager
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        /*
         * Construtor Parametrizado
         * 
         * @param context - Contexto da BD
         * @param userManager - UserManager
         */
        public AulasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Aulas
        /*
         * Endpoint de Seleção Genérica das Graus
         * Estado: ✓
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aulas>>> GetAulas()
        {
            // Listagem das Aulas registadas na BD
            var aulas = await _context.Aulas
                .Include(a => a.Cadeira)
                .Include(a => a.Tipologia)
                .Include(a => a.Turma)
                .Include(a => a.Professor)
                .ToListAsync();
            // Retorno da Lista de Aulas
            return Ok(aulas);
        }

        // GET: api/Aulas/5
        /**
        * Endpoint destinado à Seleção de Aulas segundo o seu ID
        * Estado: ✓
        * 
        * @param id - ID da Aula
        */
        [HttpGet("{id}")]
        public async Task<ActionResult<Aulas>> GetAulas(int id)
        {
            // Pesquisa da Aula segundo o seu ID
            var aula = await _context.Aulas
                .Include(a => a.Cadeira)
                .Include(a => a.Tipologia)
                .Include(a => a.Turma)
                .Include(a => a.Professor)
                .FirstOrDefaultAsync(a => a.Id == id);
            // Caso em que não foi encontrada nenhuma Turma segundo o parâmetro recebido
            if (aula == null)
            {
                return NotFound();
            }
            // Caso em que foi encontrada pelo menos uma Turma segundo o parâmetro recebido
            return Ok(aula);
        }

        // POST: api/Aulas
        /**
         * Endpoint de Inserção de Aulas
         * Estado: ✓
         * 
         * @param aulas - Objeto do Tipo "Aulas" a ser inserido
         */
        [HttpPost]
        public async Task<ActionResult<Aulas>> PostAulas(Aulas aulas)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do Utilizador segundo a Chave Estrangeira "ProfessorFK"
            var user = await _userManager.FindByIdAsync(aulas.ProfessorFK);
            // Verificação se o Utilizador existe e se o mesmo está no role "Docente"
            if (user == null || !await _userManager.IsInRoleAsync(user, "Docente"))
            {
                return BadRequest("ProfessorFK deve referir-se a um utilizador com o papel 'Docente'");
            }
            // Caso em que o Utilizador existe e está no role "Docente"
            _context.Aulas.Add(aulas);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAulas), new { id = aulas.Id }, aulas);
        }

        // PUT: api/Aulas/5
        /**
         * Endpoint para Atualizar as Aulas
         * Estado: ✓
         * 
         * @param id - ID da Aula
         * @param aulas - Objeto do tipo "Aulas" a ser atualizado
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAulas(int id, Aulas aulas)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o ID fornecido é distinto da Aula fornecida
            if (id != aulas.Id)
            {
                return BadRequest();
            }
            // Atualização dos estado da entidade a ser atualizada
            _context.Entry(aulas).State = EntityState.Modified;
            try
            {
                // Tentativa da Atualização da Aula
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Caso em que não foi encontrado uma Aula com o ID fornecido
                if (!AulasExists(id))
                {
                    return NotFound();
                }
                // Caso em que foi encontrado uma Aula com o ID fornecido
                else
                {
                    throw;
                }
            }
            // Atualização realizada com sucesso
            return NoContent();
        }

        // DELETE: api/Aulas/5
        /**
         * Endpoint de Apagamento das Aulas
         * Estado: ✓
         * 
         * @param id - ID da Aula
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAulas(int id)
        {
            // Pesquisa da Aula pelo seu ID
            var aula = await _context.Aulas.FindAsync(id);
            // Caso em não foi encontrada a Aula pretendida
            if (aula == null)
            {
                return NotFound();
            }
            // Apaga a mesma e salva as alterações na BD
            _context.Aulas.Remove(aula);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
         * Método para Verificação da Existência de uma Aula
         * Estado: ✓
         * 
         * @param id - ID da Aula
         */
        private bool AulasExists(int id)
        {
            return _context.Aulas.Any(e => e.Id == id);
        }
    }
}