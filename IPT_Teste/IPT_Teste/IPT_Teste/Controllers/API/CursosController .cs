using IPT_Teste.Data;
using IPT_Teste.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
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
        public CursosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Cursos
        /*
         * Endpoint de Seleção Genérica dos Cursos
         * Estado: ✓
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cursos>>> GetCursos()
        {
            // Retorno da Lista de Cursos
            return await _context.Cursos
                .Include(c => c.AnoLetivo)
                .Include(c => c.Instituicao)
                .Include(c => c.Grau)
                .Include(c => c.Professor)
                .ToListAsync();
        }

        // GET: api/Cursos/5
        /**
         * Endpoint destinado à Seleção de Cursos segundo o seu ID
         * Estado: ✓
         * 
         * @param id - ID do Curso
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<Cursos>> GetCurso(int id)
        {
            // Pesquisa do Curso segundo o seu ID
            var curso = await _context.Cursos
                .Include(c => c.AnoLetivo)
                .Include(c => c.Instituicao)
                .Include(c => c.Grau)
                .Include(c => c.Professor)
                .FirstOrDefaultAsync(c => c.Id == id);
            // Caso em que não foi encontrado nenhum Curso segundo o parâmetro recebido
            if (curso == null)
            {
                return NotFound();
            }
            // Caso em que foi encontrado pelo menos um Curso segundo o parâmetro recebido
            return curso;
        }

        // POST: api/Cursos
        /**
         * Endpoint de Inserção de Cursos
         * Estado: ✓
         * 
         * @param curso - Objeto do Tipo "Cursos" a ser inserido
         */
        [HttpPost]
        public async Task<ActionResult<Cursos>> PostCurso(Cursos curso)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do Utilizador segundo a Chave Estrangeira "ProfessorFK"
            var user = await _userManager.FindByIdAsync(curso.ProfessorFK);
            // Verificação se o Utilizador existe e se o mesmo está no role "Coordenador de Curso"
            if (user == null || !await _userManager.IsInRoleAsync(user, "Coordenador de Curso"))
            {
                return BadRequest("ProfessorFK deve referir-se a um utilizador com o papel 'Coordenador de Curso'");
            }
            // Caso em que o Utilizador existe e está no role "Coordenador de Curso"
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }

        // PUT: api/Cursos/5
        /**
         * Endpoint para Atualizar os Cursos
         * Estado: ✓
         * 
         * @param id - ID do Curso
         * @param curso - Objeto do tipo "Cursos" a ser atualizado
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Cursos curso)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o ID fornecido é distinto do Curso fornecido
            if (id != curso.Id)
            {
                return BadRequest();
            }
            // Obtenção do Utilizador segundo a chave estrangeira "ProfessorFK"
            var user = await _userManager.FindByIdAsync(curso.ProfessorFK);
            // Verificação se o Utilizador existe e está no role "Coordenador de Curso"
            if (user == null || !await _userManager.IsInRoleAsync(user, "Coordenador de Curso"))
            {
                return BadRequest("ProfessorFK deve referir-se a um utilizador com o papel 'Coordenador de Curso'");
            }
            // Atualização dos estado da entidade a ser atualizada
            _context.Entry(curso).State = EntityState.Modified;
            try
            {
                // Tentativa da Atualização do Curso
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Caso em que não foi encontrado um Curso com o ID fornecido
                if (!CursoExists(id))
                {
                    return NotFound();
                }
                // Caso em que foi encontrado um Curso com o ID fornecido
                else
                {
                    throw;
                }
            }
            // Atualização realizada com sucesso
            return NoContent();
        }

        // DELETE: api/Cursos/5
        /**
         * Endpoint de Apagamento dos Cursos
         * Estado: ✓
         * 
         * @param id - ID do Curso
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            // Pesquisa do Curso pelo seu ID
            var curso = await _context.Cursos.FindAsync(id);
            // Caso em não foi encontrado o Curso pretendido
            if (curso == null)
            {
                return NotFound();
            }
            // Apaga omesmo e salva as alterações na BD
            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Cursos/coordenadores
        /*
         * Endpoint de Seleção Genérica dos Coordenadores de Curso
         * Estado: ✓
         */
        [HttpGet("coordenadores")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetCoordenadores()
        {
            var coordenadores = await _userManager.GetUsersInRoleAsync("Coordenador de Curso");
            return Ok(coordenadores);
        }

        /**
         * Método para Verificação da Existência de um Curso
         * Estado: ✓
         * 
         * @param id - ID do Curso
         */
        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }

        // GET: api/Cursos/5/cadeiras
        /**
        * Endpoint destinado à Seleção de Cadeiras de um Curso segundo o seu ID
        * Estado: ✓
        * 
        * @param id - ID do Curso
        */
        [HttpGet("{id}/cadeiras")]
        public async Task<ActionResult<IEnumerable<Cadeiras>>> GetCadeirasDoCurso(int id)
        {
            // Pesquisa do Curso segundo o seu ID
            var curso = await _context.Cursos
                .Include(c => c.Cadeiras)
                .FirstOrDefaultAsync(c => c.Id == id);
            // Caso em que não foi encontrada nenhum Curso segundo o parâmetro recebido
            if (curso == null)
            {
                return NotFound();
            }
            // Caso em que foi encontrado pelo menos um Curso segundo o parâmetro recebido
            return Ok(curso.Cadeiras);
        }

        // POST: api/Cursos/5/cadeiras/10
        /**
         * Endpoint de Inserção de Cadeiras num dado Curso
         * Estado: ✓
         * 
         * @param cursoId - ID do Curso
         * @param cadeiraId - ID da Cadeira 
         */
        [HttpPost("{cursoId}/cadeiras/{cadeiraId}")]
        public async Task<IActionResult> AdicionarCadeiraAoCurso(int cursoId, int cadeiraId)
        {
            // Pesquisa do Curso segundo o seu ID
            var curso = await _context.Cursos
                .Include(c => c.Cadeiras)
                .FirstOrDefaultAsync(c => c.Id == cursoId);
            // Pesquisa da Cadeira segundo o seu ID
            var cadeira = await _context.Cadeiras.FindAsync(cadeiraId);
            // Caso em que ou não existe Curso ou Cadeira com os IDs fornecidos
            if (curso == null || cadeira == null)
            {
                return NotFound("Curso ou cadeira não encontrados.");
            }
            // Caso em que a Cadeira a ser adicionada já se encontra associada ao Curso
            if (curso.Cadeiras.Any(c => c.Id == cadeiraId))
            {
                return BadRequest("A cadeira já está associada ao curso.");
            }
            // Caso em que a Cadeira não está associada ao Curso 
            curso.Cadeiras.Add(cadeira);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/Cursos/5/cadeiras/10
        /**
         * Endpoint de Apagamento de Cadeiras num dado Curso
         * Estado: ✓
         * 
         * @param cursoId - ID do Curso
         * @param cadeiraId - ID da Cadeira
         */
        [HttpDelete("{cursoId}/cadeiras/{cadeiraId}")]
        public async Task<IActionResult> RemoverCadeiraDoCurso(int cursoId, int cadeiraId)
        {
            // Pesquisa do Curso segundo o seu ID
            var curso = await _context.Cursos
                .Include(c => c.Cadeiras)
                .FirstOrDefaultAsync(c => c.Id == cursoId);
            // Pesquisa da Cadeira segundo o seu ID
            var cadeira = await _context.Cadeiras.FindAsync(cadeiraId);
            // Caso em que ou não existe Curso ou Cadeira com os IDs fornecidos
            if (curso == null || cadeira == null)
            {
                return NotFound("Curso ou cadeira não encontrados.");
            }
            // Caso em que a Cadeira a ser adicionada não se encontra associada ao Curso
            if (!curso.Cadeiras.Any(c => c.Id == cadeiraId))
            {
                return BadRequest("A cadeira não está associada ao curso.");
            }
            // Caso em que a Cadeira está associada ao Curso 
            curso.Cadeiras.Remove(cadeira);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: api/Cursos/anoletivo/3
        /**
        * Endpoint destinado à Seleção de Cursos de um dado Ano Letivo
        * Estado: ✓
        * 
        * @param anoLetivo - ID do Ano Letivo
        */
        [HttpGet("anoletivo/{anoLetivo}")]
        public async Task<ActionResult<IEnumerable<Cursos>>> GetCursosPorAnoLetivo(int anoLetivo)
        {
            // Retorno da Lista de Cursos do dado Ano Letivo
            return await _context.Cursos
                .Include(c => c.AnoLetivo)
                .Include(c => c.Instituicao)
                .Include(c => c.Grau)
                .Include(c => c.Professor)
                .Where(c => c.AnoLetivoFK == anoLetivo)
                .ToListAsync();
        }

        // GET: api/Cursos/grau/3
        /**
        * Endpoint destinado à Seleção de Cursos de um dado Grau
        * Estado: ✓
        * 
        * @param grauId - ID do Grau
        */
        [HttpGet("grau/{grauId}")]
        public async Task<ActionResult<IEnumerable<Cursos>>> GetCursosPorGrau(int grauId)
        {
            // Retorno da Lista de Cursos do dado Grau
            return await _context.Cursos
                .Include(c => c.AnoLetivo)
                .Include(c => c.Instituicao)
                .Include(c => c.Grau)
                .Include(c => c.Professor)
                .Where(c => c.GrauFK == grauId)
                .ToListAsync();
        }

    }
}
