using IPT_Teste.Data;
using IPT_Teste.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPT_Teste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Adição da Injeção da Base de Dados e manipulação de utilizadores
        /// </summary>
        /// <param name="context">variável de conexão</param>
        /// <param name="userManager">variável de interação com utilizadores</param>
        public ProfessoresController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Professores
        /// <summary>
        /// Endpoint de Listagem de todos os professores (docentes)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetAllDocentes()
        {
            // Guarda os utilizadores existentes na variável allUsers
            var allUsers = _userManager.Users.ToList();
            // Guarda uma lista vazia do tipo IdentityUser
            var docentes = new List<IdentityUser>();

            // Itera sobre todos os utilizadores e guarda na lista docentes apenas os que têm o papel de "Docente"
            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Docente"))
                {
                    docentes.Add(user);
                }
            }

            // Retorna a lista de docentes
            return Ok(docentes);
        }

        // GET: api/Professores/curso/5
        /// <summary>
        /// Endpoint de Listagem de professores (docentes) associados a um curso específico
        /// </summary>
        /// <param name="cursoId"></param>
        /// <returns></returns>
        [HttpGet("curso/{cursoId}")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetProfessoresByCurso(int cursoId)
        {
            // Pesquisa de professores associados ao curso com o id fornecido
            var professores = await _context.Professores
                .Where(p => p.CursoFK == cursoId)
                .Include(p => p.Professor)
                .Select(p => p.Professor)
                .ToListAsync();

            // Retorna a pesquisa feita
            return Ok(professores);
        }

        // POST: api/Professores
        /// <summary>
        /// Endpoint de Adição de um professor (docente) a um curso específico
        /// </summary>
        /// <param name="model">conector da classe professor</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddProfessorToCurso([FromBody] Professores model)
        {
            // variável que guarda o curso associado ao professor
            var curso = await _context.Cursos.FindAsync(model.CursoFK);
            // Verifica se o curso existe
            if (curso == null)
                return NotFound("Curso não encontrado");

            // variável que guarda o utilizador associado ao professor
            var user = await _userManager.FindByIdAsync(model.ProfessorFK);
            // Verifica se o utilizador existe
            if (user == null)
                return NotFound("Usuário não encontrado");

            // variável que guarda os papéis do utilizador
            var roles = await _userManager.GetRolesAsync(user);
            // Verifica se o utilizador tem o papel de "Docente"
            if (!roles.Contains("Docente"))
                return BadRequest("Usuário não tem o papel de docente");

            // Verifica se o docente já está associado ao curso
            bool jaExiste = await _context.Professores
                .AnyAsync(p => p.CursoFK == model.CursoFK && p.ProfessorFK == model.ProfessorFK);
            if (jaExiste)
                return BadRequest("Esse docente já está associado a este curso");

            // Adiciona o professor ao curso e salva as alterações no contexto
            _context.Professores.Add(model);
            await _context.SaveChangesAsync();

            // Retorna uma resposta de sucesso
            return Ok(new { mensagem = "Docente associado ao curso com sucesso" });
        }


        // DELETE: api/Professores?cursoId=5&professorId=abc123
        /// <summary>
        /// Endpoint de Eliminação de um professor (docente) de um curso específico
        /// </summary>
        /// <param name="cursoId">identificador de um curso</param>
        /// <param name="professorId">identificador de um professor</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> RemoveProfessorFromCurso([FromQuery] int cursoId, [FromQuery] string professorId)
        {
            // Verifica se o professor está associado ao curso
            var association = await _context.Professores
                .FirstOrDefaultAsync(p => p.CursoFK == cursoId && p.ProfessorFK == professorId);

            // Se não existir associação entre o professor e o curso, retorna NotFound
            if (association == null)
                return NotFound("Associação não encontrada");

            // Remove a associação do professor com o curso e salva as alterações no contexto
            _context.Professores.Remove(association);
            await _context.SaveChangesAsync();

            // Retorna uma resposta de sucesso
            return Ok(new { mensagem = "Professor removido do curso com sucesso"});
        }
    }
}