using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Models;
using IPT_Teste.Data;
using IPT_Teste.Models.DTOs;
using Azure.Core;
using Microsoft.AspNetCore.Identity;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HorariosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Horarios
        /// <summary>
        /// Endpoint de Listagem de Horários e Blocos associados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Horarios>>> GetHorarios()
        {
            // Retorna uma lista de horários, incluindo os blocos associados a cada horário
            var horarios = await _context.Horarios.Include(h => h.Blocos).ToListAsync();
            return Ok(horarios);
        }

        // GET: api/Horarios/5
        /// <summary>
        /// Endpoint de Listagem de um Horário e Blocos associados
        /// </summary>
        /// <param name="id">identificador de um horário</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Horarios>> GetHorario(int id)
        {
            // Pesquisa de horário e blocos associados de acordo com o id fornecido
            var horario = await _context.Horarios
                .Include(h => h.Blocos)
                .FirstOrDefaultAsync(h => h.Id == id);

            // Se não existir horário com o id fornecido, retorna NotFound
            if (horario == null)
            {
                return NotFound();
            }

            return Ok(horario);
        }

        [HttpGet("pendentes")]
        public async Task<ActionResult<Horarios>> GetHorariosByUser([FromQuery]string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Utilizador nao encontrado");

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("Admistrador") && !roles.Contains("Comissão de Horários") && !roles.Contains("Diretor/a"))
            {
                return BadRequest(); 
            }

            var horariosPendentes = await _context.Horarios
                .Where(h => (int) h.Estado == 1)
                .Include(h => h.Turma.Curso)
                .ToListAsync();

            return Ok(horariosPendentes);
        }

        [HttpPost("horarios-turma")]
        public async Task<ActionResult> GetHorarioTurma([FromBody] int id)
        {
            var turmaExiste = await _context.Turmas.AnyAsync(t => t.Id == id);
            if (!turmaExiste)
                return NotFound("Turma não encontrada.");

            var horario = await _context.Horarios. 
                Where(h => h.TurmaFK == id).
                Include(h => h.Turma.Curso.Professor).
                ToListAsync();

            return Ok(horario);
            
        }

        [HttpPost("SetStatus/{id}/{status}")]
        public async Task<ActionResult> SetStatus(int id, int status)
        {

            // Fake database lookup
            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound($"Horario {id} não encontrado.");
            }

            horario.Estado = (EstadoHorario)status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Horarios
        /// <summary>
        /// Endpoint de Criação de um Horário
        /// </summary>
        /// <param name="horario">conector da classe horário</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Horarios>> PostHorario(Horario2DTO horario)
        {
            var bloco = await _context.Blocos. 
                Where(b => b.Id == 94). 
                ToListAsync();

            var horariofin = new Horarios
            {
                Inicio = horario.Inicio,
                Fim = horario.Fim,
                TurmaFK = horario.TurmaFK,
                Estado = EstadoHorario.EDITAVEL,
                Blocos = bloco,
            };
            
            _context.Horarios.Add(horariofin);
            await _context.SaveChangesAsync();

            // Retorna uma resposta de sucesso com o horário criada
            return CreatedAtAction(nameof(GetHorario), new { id = horario.Id }, horario);
        }

        // PUT: api/Horarios/5
        /// <summary>
        /// Endpoint de Atualização de um Horário
        /// </summary>
        /// <param name="id">identificador de um horário</param>
        /// <param name="horario">conector da classe horario</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHorario(int id, Horarios horario)
        {
            // Verifica se o id fornecido já existe na base de dados
            if (id != horario.Id)
            {
                return BadRequest();
            }

            // Marca o horário como modificado no contexto da base de dados
            _context.Entry(horario).State = EntityState.Modified;

            // Salva as alterações na base de dados se não houver erros de concorrência
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorarioExists(id))
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

        // DELETE: api/Horarios/5
        /// <summary>
        /// Endpoint de Eliminação de um Horário
        /// </summary>
        /// <param name="id">identificador de um horário</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            // Pesquisa de horário de acordo com o id fornecido
            var horario = await _context.Horarios.FindAsync(id);

            // Se não existir horário com o id fornecido, retorna NotFound
            if (horario == null)
            {
                return NotFound();
            }

            // Elimina o horário da base de dados e salva as alterações
            _context.Horarios.Remove(horario);
            await _context.SaveChangesAsync();

            // Retorna uma resposta de sucesso sem conteúdo
            return NoContent();
        }

        /// <summary>
        /// Verifica se um Horário existe na Base de Dados
        /// </summary>
        /// <param name="id">identificador de um horário</param>
        /// <returns></returns>
        private bool HorarioExists(int id)
        {
            return _context.Horarios.Any(e => e.Id == id);
        }

        // GET: api/Horarios/5/blocos
        /// <summary>
        /// Endpoint de Listagem de Blocos associados a um Horário específico
        /// </summary>
        /// <param name="id">identificador de um horário</param>
        /// <returns></returns>
        [HttpGet("{id}/blocos")]
        public async Task<ActionResult<IEnumerable<Blocos>>> GetBlocosDoHorario(int id)
        {
            // Pesquisa de horário e blocos associados de acordo com o id fornecido
            var horario = await _context.Horarios
                .Include(h => h.Blocos)
                .FirstOrDefaultAsync(h => h.Id == id);

            // Se não existir horário com o id fornecido, retorna NotFound
            if (horario == null)
            {
                return NotFound();
            }

            // Retorna os blocos associados ao horário encontrado
            return Ok(horario.Blocos);
        }

        // POST: api/Horarios/5/blocos/10
        /// <summary>
        /// Endpoint que Adiciona um Bloco a um Horário específico
        /// </summary>
        /// <param name="horarioId">identificador de um horário</param>
        /// <param name="blocoId">identificador de um bloco</param>
        /// <returns></returns>
        [HttpPost("{horarioId}/blocos/{blocoId}")]
        public async Task<IActionResult> AdicionarBlocoAoHorario(int horarioId, int blocoId)
        {
            // Pesquisa de horário e bloco de acordo com os ids fornecidos
            var horario = await _context.Horarios
                .Include(h => h.Blocos)
                .FirstOrDefaultAsync(h => h.Id == horarioId);
            var bloco = await _context.Blocos.FindAsync(blocoId);

            // Verifica se o horário ou bloco existe
            if (horario == null || bloco == null)
            {
                return NotFound("Horário ou bloco não encontrados.");
            }

            // Verifica se o bloco já está associado ao horário
            if (horario.Blocos.Any(b => b.Id == blocoId))
            {
                return BadRequest("O bloco já está associado ao horário.");
            }

            // Adiciona o bloco ao horário e salva as alterações
            horario.Blocos.Add(bloco);
            await _context.SaveChangesAsync();

            // Retorna uma resposta de sucesso
            return Ok();
        }

        // DELETE: api/Horarios/5/blocos/10
        /// <summary>
        /// Endpoint que Remove um Bloco de um Horário específico
        /// </summary>
        /// <param name="horarioId">identificador de um horário</param>
        /// <param name="blocoId">identificador de um bloco</param>
        /// <returns></returns>
        [HttpDelete("{horarioId}/blocos/{blocoId}")]
        public async Task<IActionResult> RemoverBlocoDoHorario(int horarioId, int blocoId)
        {
            // Pesquisa de horário e bloco de acordo com os ids fornecidos
            var horario = await _context.Horarios
                .Include(h => h.Blocos)
                .FirstOrDefaultAsync(h => h.Id == horarioId);
            var bloco = await _context.Blocos.FindAsync(blocoId);

            // Verifica se o horário ou bloco existe
            if (horario == null || bloco == null)
            {
                return NotFound("Horário ou bloco não encontrados.");
            }

            // Verifica se o bloco está associado ao horário
            if (!horario.Blocos.Any(b => b.Id == blocoId))
            {
                return BadRequest("O bloco não está associado ao horário.");
            }

            // Remove o bloco do horário e salva as alterações
            horario.Blocos.Remove(bloco);
            await _context.SaveChangesAsync();

            // Retorna uma resposta de sucesso
            return Ok();
        }
    }
}
