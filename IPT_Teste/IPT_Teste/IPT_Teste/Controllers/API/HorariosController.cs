using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Models;
using IPT_Teste.Data;
using IPT_Teste.Models.DTOs;
using Azure.Core;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HorariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Horarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Horarios>>> GetHorarios()
        {
            var horarios = await _context.Horarios.Include(h => h.Blocos).ToListAsync();
            return Ok(horarios);
        }

        // GET: api/Horarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Horarios>> GetHorario(int id)
        {
            var horario = await _context.Horarios
                .Include(h => h.Blocos)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (horario == null)
            {
                return NotFound();
            }

            return Ok(horario);
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

            return CreatedAtAction(nameof(GetHorario), new { id = horario.Id }, horario);
        }

        // PUT: api/Horarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHorario(int id, Horarios horario)
        {
            if (id != horario.Id)
            {
                return BadRequest();
            }

            _context.Entry(horario).State = EntityState.Modified;

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

        // DELETE: api/Horarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }

            _context.Horarios.Remove(horario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HorarioExists(int id)
        {
            return _context.Horarios.Any(e => e.Id == id);
        }

        // GET: api/Horarios/5/blocos
        [HttpGet("{id}/blocos")]
        public async Task<ActionResult<IEnumerable<Blocos>>> GetBlocosDoHorario(int id)
        {
            var horario = await _context.Horarios
                .Include(h => h.Blocos)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (horario == null)
            {
                return NotFound();
            }

            return Ok(horario.Blocos);
        }

        // POST: api/Horarios/5/blocos/10
        [HttpPost("{horarioId}/blocos/{blocoId}")]
        public async Task<IActionResult> AdicionarBlocoAoHorario(int horarioId, int blocoId)
        {
            var horario = await _context.Horarios
                .Include(h => h.Blocos)
                .FirstOrDefaultAsync(h => h.Id == horarioId);
            var bloco = await _context.Blocos.FindAsync(blocoId);

            if (horario == null || bloco == null)
            {
                return NotFound("Horário ou bloco não encontrados.");
            }

            if (horario.Blocos.Any(b => b.Id == blocoId))
            {
                return BadRequest("O bloco já está associado ao horário.");
            }

            horario.Blocos.Add(bloco);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Horarios/5/blocos/10
        [HttpDelete("{horarioId}/blocos/{blocoId}")]
        public async Task<IActionResult> RemoverBlocoDoHorario(int horarioId, int blocoId)
        {
            var horario = await _context.Horarios
                .Include(h => h.Blocos)
                .FirstOrDefaultAsync(h => h.Id == horarioId);
            var bloco = await _context.Blocos.FindAsync(blocoId);

            if (horario == null || bloco == null)
            {
                return NotFound("Horário ou bloco não encontrados.");
            }

            if (!horario.Blocos.Any(b => b.Id == blocoId))
            {
                return BadRequest("O bloco não está associado ao horário.");
            }

            horario.Blocos.Remove(bloco);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
