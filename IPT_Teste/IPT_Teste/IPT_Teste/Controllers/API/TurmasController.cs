using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Models;
using IPT_Teste.Data;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurmasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Adição da Injeção da Base de Dados
        /// </summary>
        /// <param name="context">Variável de conexão</param>
        public TurmasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Turmas
        /// <summary>
        /// Endpoint de Listagem de Turmas e Curso associado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turmas>>> GetTurmas()
        {
            // Retorna uma lista de turmas, incluindo o curso associado a cada turma
            return await _context.Turmas.Include(t => t.Curso).ToListAsync();
        }

        // GET: api/Turmas/5
        /// <summary>
        /// Endpoint de Listagem de uma Turma e Curso associado
        /// </summary>
        /// <param name="id">identificador de uma turma</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Turmas>> GetTurma(int id)
        {
            // Pesquisa de turma e curso associado de acordo com o id de turma fornecidos
            var turma = await _context.Turmas.Include(t => t.Curso)
                                             .FirstOrDefaultAsync(t => t.Id == id);
            
            // Se não existir turma com o id fornecido, retorna NotFound
            if (turma == null)
            {
                return NotFound();
            }

            // Retorna a turma encontrada
            return turma;
        }

        // POST: api/Turmas
        /// <summary>
        /// Endpoint de Criação de uma Turma
        /// </summary>
        /// <param name="turma">conector da classe turma</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Turmas>> PostTurma(Turmas turma)
        {
            // Verifica se a turma a ser criada já existe de acordo com o ID e a FK do curso
            bool exists = await _context.Turmas.AnyAsync(t => t.Id == turma.Id && t.CursoFK == turma.CursoFK);

            // Se já existir a turma, retorna BadRequest
            if (exists)
            {
                return BadRequest("Já existe uma turma com este ID para o curso especificado.");
            }

            // Adiciona uma nova turma e salva as alterações no contexto da base de dados
            _context.Turmas.Add(turma);
            await _context.SaveChangesAsync();

            // Retorna o resultado da criação da turma, incluindo o local onde a turma foi criada
            return CreatedAtAction("GetTurma", new { id = turma.Id }, turma);
        }

        // PUT: api/Turmas/5
        /// <summary>
        /// Endpoint de Atualização de uma Turma
        /// </summary>
        /// <param name="id">identificador de uma turma</param>
        /// <param name="turma">conector da classe turma</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTurma(int id, Turmas turma)
        {
            // Verifica se o ID fornecido já existe na base de dados
            if (id != turma.Id)
            {
                return BadRequest();
            }

            // Apresenta na Base de Dados que a turma foi modificada
            _context.Entry(turma).State = EntityState.Modified;

            // Salva as alterações no contexto da base de dados se não houver nenhum erro de concorrência
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TurmaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Retorna NoContent se a atualização for bem-sucedida
            return NoContent();
        }

        // DELETE: api/Turmas/5
        /// <summary>
        /// Endpoint de Eliminação de uma Turma
        /// </summary>
        /// <param name="id">identificador de uma turma</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTurma(int id)
        {
            // Pesquisa de turma de acordo com o id fornecido
            var turma = await _context.Turmas.FindAsync(id);
            
            // Se não existir turma com o id fornecido, retorna NotFound
            if (turma == null)
            {
                return NotFound();
            }

            // Elimina a turma encontrada e salva as alterações no contexto da base de dados
            _context.Turmas.Remove(turma);
            await _context.SaveChangesAsync();

            // Retorna NoContent se a atualização for bem-sucedida
            return NoContent();
        }

        /// <summary>
        /// Verifica se uma Turma existe na Base de Dados
        /// </summary>
        /// <param name="id">identificador de uma turma</param>
        /// <returns></returns>
        private bool TurmaExists(int id)
        {
            return _context.Turmas.Any(e => e.Id == id);
        }
    }
}