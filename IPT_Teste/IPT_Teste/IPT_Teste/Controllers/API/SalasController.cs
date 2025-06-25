using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Models;
using System;
using IPT_Teste.Data;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Adição da Injeção da Base de Dados
        /// </summary>
        /// <param name="context">Variável de conexão</param>
        public SalasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Salas
        /// <summary>
        /// Endpoint de Listagem de Salas e Localidade associada
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salas>>> GetSalas()
        {
            // Retorna uma lista de salas, incluindo a localidade associada a cada sala
            return await _context.Salas
                                 .Include(i => i.Localidade)
                                 .ToListAsync();
        }

        // GET: api/Salas/5
        /// <summary>
        /// Endpoint de Listagem de uma Sala e Localidade associada
        /// </summary>
        /// <param name="id">identificador de uma sala</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Salas>> GetSala(int id)
        {
            // Pesquisa de sala e localidade associada de acordo com o id fornecido
            var sala = await _context.Salas
                                            .Include(i => i.Localidade)
                                            .FirstOrDefaultAsync(i => i.Id == id);

            // Se não existir sala com o id fornecido, retorna NotFound
            if (sala == null)
            {
                return NotFound();
            }

            // Retorna a sala encontrada
            return sala;
        }

        // POST: api/Salas
        /// <summary>
        /// Endpoint de Criação de uma Sala
        /// </summary>
        /// <param name="sala">conector da classe sala</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Salas>> PostSala(Salas sala)
        {
            // Verifica se a Localidade existe
            var localidade = await _context.Localidades.FindAsync(sala.LocalidadeFK);

            // Se não existir localidade com a FK fornecida, retorna BadRequest
            if (localidade == null)
            {
                return BadRequest("Localidade não encontrada.");
            }

            // Adiciona a nova sala à base de dados e salva as alterações
            _context.Salas.Add(sala);
            await _context.SaveChangesAsync();

            // Retorna a sala criada com o local apropriado
            return CreatedAtAction(nameof(GetSala), new { id = sala.Id }, sala);
        }

        // PUT: api/Salas/5
        /// <summary>
        /// Endpoint de Atualização de uma Sala
        /// </summary>
        /// <param name="id">identificador de uma sala</param>
        /// <param name="sala">conector da classe sala</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSala(int id, Salas sala)
        {
            // Verifica se o id fornecido corresponde ao id da sala a ser atualizada
            if (id != sala.Id)
            {
                return BadRequest();
            }

            // Confirma se a localidade referenciada existe
            if (!_context.Localidades.Any(l => l.Id == sala.LocalidadeFK))
            {
                return BadRequest("Localidade não encontrada.");
            }

            // Marca a sala como modificada no contexto da base de dados
            _context.Entry(sala).State = EntityState.Modified;

            // Salva as alterações no contexto da base de dados se não houver nenhum erro de concorrência 
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaExists(id))
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

        // DELETE: api/Salas/5
        /// <summary>
        /// Endpoint de Eliminação de uma Sala
        /// </summary>
        /// <param name="id">identicador de uma sala</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSala(int id)
        {
            // Pesquisa de sala de acordo com o id fornecido
            var sala = await _context.Salas.FindAsync(id);

            // Se não existir sala com o id fornecido, retorna NotFound
            if (sala == null)
            {
                return NotFound();
            }

            // Elimina a sala encontrada e salva as alterações no contexto da base de dados
            _context.Salas.Remove(sala);
            await _context.SaveChangesAsync();

            // Retorna NoContent se a atualização for bem-sucedida
            return NoContent();
        }

        /// <summary>
        /// Verifica se uma Sala existe na Base de Dados
        /// </summary>
        /// <param name="id">identicador de uma sala</param>
        /// <returns></returns>
        private bool SalaExists(int id)
        {
            return _context.Salas.Any(e => e.Id == id);
        }
    }
}
