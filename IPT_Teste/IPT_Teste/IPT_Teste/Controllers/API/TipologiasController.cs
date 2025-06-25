using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Data;
using IPT_Teste.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipologiasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Adição da Injeção da Base de Dados
        /// </summary>
        /// <param name="context">Variável de conexão</param>
        public TipologiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Tipologias
        /// <summary>
        /// Endpoint de Listagem de Tipologias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tipologias>>> GetTipologias()
        {
            return await _context.Tipologias.ToListAsync();
        }

        // GET: api/Tipologias/5
        /// <summary>
        /// Endpoint de Listagem de uma Tipologia
        /// </summary>
        /// <param name="id">identificador de uma tipologia</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Tipologias>> GetTipologia(int id)
        {
            // Pesquisa de tipologia de acordo com o id fornecido
            var tipologia = await _context.Tipologias.FindAsync(id);

            // Se não existir tipologia com o id fornecido, retorna NotFound
            if (tipologia == null)
            {
                return NotFound();
            }

            // Retorna a tipologia encontrada
            return tipologia;
        }

        // POST: api/Tipologias
        /// <summary>
        /// Endpoint de Criação de uma Tipologia
        /// </summary>
        /// <param name="tipologia">conector da classe tipologia</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Tipologias>> PostTipologia(Tipologias tipologia)
        {
            // Verifica se a tipologia a ser criada já existe de acordo com o Tipologia(Nome) da tipologia
            bool exists = await _context.Tipologias.AnyAsync(t => t.Tipologia == tipologia.Tipologia);

            // Se já existir a tipologia, retorna BadRequest
            if (exists)
            {
                return BadRequest("Já existe uma tipologia de acordo com o seu nome.");
            }

            // Adiciona a nova tipologia ao contexto e salva as alterações
            _context.Tipologias.Add(tipologia);
            await _context.SaveChangesAsync();

            // Retorna o resultado da criação com o local onde a nova tipologia pode ser obtida
            return CreatedAtAction(nameof(GetTipologia), new { id = tipologia.Id }, tipologia);
        }

        // PUT: api/Tipologias/5
        /// <summary>
        /// Endpoint de Atualização de uma Tipologia
        /// </summary>
        /// <param name="id">identificador de uma tipologia</param>
        /// <param name="tipologia">conector da classe tipologia</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipologia(int id, Tipologias tipologia)
        {
            // Verifica se o id fornecido existe na Base de dados
            if (id != tipologia.Id)
            {
                return BadRequest();
            }

            // Apresenta na Base de Dados que a tipologia foi modificada
            _context.Entry(tipologia).State = EntityState.Modified;

            // Salva as alterações no contexto da base de dados se não houver nenhum erro de concorrência 
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipologiaExists(id))
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

        // DELETE: api/Tipologias/5
        /// <summary>
        /// Endpoint de Eliminação de uma Tipologia
        /// </summary>
        /// <param name="id">identificador de uma tipologia</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipologia(int id)
        {
            // Pesquisa de tipologia de acordo com o id fornecido
            var tipologia = await _context.Tipologias.FindAsync(id);

            // Se não existir tipologia com o id fornecido, retorna NotFound
            if (tipologia == null)
            {
                return NotFound();
            }

            // Elimina a tipologia encontrada e salva as alterações
            _context.Tipologias.Remove(tipologia);
            await _context.SaveChangesAsync();

            // Retorna NoContent se a atualização for bem-sucedida
            return NoContent();
        }

        /// <summary>
        /// Verifica se uma Tipologia existe na Base de Dados
        /// </summary>
        /// <param name="id">identificador de uma tipologia</param>
        /// <returns></returns>
        private bool TipologiaExists(int id)
        {
            return _context.Tipologias.Any(e => e.Id == id);
        }
    }
}