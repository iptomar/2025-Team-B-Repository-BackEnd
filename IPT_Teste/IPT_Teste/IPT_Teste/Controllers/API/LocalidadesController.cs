using Microsoft.AspNetCore.Mvc;
using IPT_Teste.Models;
using Microsoft.EntityFrameworkCore;
using System;
using IPT_Teste.Data;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalidadesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Adição da Injeção da Base de Dados 
        /// </summary>
        /// <param name="context">variável de conexão</param>
        public LocalidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Localidades
        /// <summary>
        /// Endpoint de Listagem de Localidades
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Localidades>>> GetLocalidades()
        {
            return await _context.Localidades.ToListAsync();
        }

        // GET: api/Localidades/5
        /// <summary>
        /// Endpoint de Listagem de uma Localidade específica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Localidades>> GetLocalidade(int id)
        {
            // Pesquisa de localidade de acordo com o id fornecido
            var localidade = await _context.Localidades.FindAsync(id);

            // Verifica se a localidade existe
            if (localidade == null)
            {
                return NotFound();
            }

            // Retorna a localidade encontrada
            return localidade;
        }

        // POST: api/Localidades
        /// <summary>
        /// Endpoint de Criação de uma Localidade
        /// </summary>
        /// <param name="localidade">conector da classe localidade</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Localidades>> PostLocalidade(Localidades localidade)
        {
            // Verifica se a localidade existe na base de dados de acordo com a Localidade(Nome) da Localidade
            bool exists = await _context.Localidades.AnyAsync(l => l.Localidade == localidade.Localidade);

            // Se já existir a localidade, retorna BadRequest
            if (exists)
            {
                return BadRequest("Já existe uma localidade de acordo com o seu nome.");
            }

            // Adiciona a nova localidade à base de dados e salva as alterações
            _context.Localidades.Add(localidade);
            await _context.SaveChangesAsync();

            // Retorna uma resposta de sucesso com a localidade criada
            return CreatedAtAction(nameof(GetLocalidade), new { id = localidade.Id }, localidade);
        }

        // PUT: api/Localidades/5
        /// <summary>
        /// Endpoint de Atualização de uma Localidade
        /// </summary>
        /// <param name="id">identificador de uma localidade</param>
        /// <param name="localidade">conector da classe localidade</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocalidade(int id, Localidades localidade)
        {
            // Verifica se o id fornecido já existe na base de dados
            if (id != localidade.Id)
            {
                return BadRequest();
            }

            // Marca a localidade como modificada no contexto da base de dados 
            _context.Entry(localidade).State = EntityState.Modified;

            // Salva as alterações no contexto da base de dados se não houver erros concorrência
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalidadeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Retorna uma resposta de sucesso sem conteúdo
            return NoContent();
        }

        // DELETE: api/Localidades/5
        /// <summary>
        /// Endpoint de Eliminação de uma Localidade
        /// </summary>
        /// <param name="id">identificador de uma localidade</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocalidade(int id)
        {
            // Pesquisa de localidade de acordo com o id fornecido
            var localidade = await _context.Localidades.FindAsync(id);

            // Verifica se a localidade existe
            if (localidade == null)
            {
                return NotFound();
            }

            // Elimina a localidade da base de dados e salva as alterações
            _context.Localidades.Remove(localidade);
            await _context.SaveChangesAsync();

            // Retorna uma resposta de sucesso sem conteúdo
            return NoContent();
        }

        /// <summary>
        /// Verifica se uma Localidade existe na Base de Dados
        /// </summary>
        /// <param name="id">identificador de uma localidade</param>
        /// <returns></returns>
        private bool LocalidadeExists(int id)
        {
            return _context.Localidades.Any(e => e.Id == id);
        }
    }
}