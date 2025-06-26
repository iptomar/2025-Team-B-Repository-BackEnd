using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPT_Teste.Models;
using System;
using IPT_Teste.Data;

namespace IPT_Teste.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstituicoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Adição da Injeção da Base de Dados 
        /// </summary>
        /// <param name="context">variável de conexão</param>
        public InstituicoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Instituicoes
        /// <summary>
        /// Endpoint de Listagem de Instituições e Localidade associada
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instituicoes>>> GetInstituicoes()
        {
            // Retorna uma lista de instituições, incluindo a localidade associada a cada instituição
            return await _context.Instituicoes
                                 .Include(i => i.Localidade)
                                 .ToListAsync();
        }

        // GET: api/Instituicoes/5
        /// <summary>
        /// Endpoint de Listagem de uma Instituição e Localidade associada
        /// </summary>
        /// <param name="id">identificador de uma instituição</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Instituicoes>> GetInstituicao(int id)
        {
            // Pesquisa de instituição e localidade associada de acordo com o id fornecido
            var instituicao = await _context.Instituicoes
                                            .Include(i => i.Localidade)
                                            .FirstOrDefaultAsync(i => i.Id == id);

            // Se não existir instituição com o id fornecido, retorna NotFound
            if (instituicao == null)
            {
                return NotFound();
            }

            // Retorna a instituição encontrada
            return instituicao;
        }

        // POST: api/Instituicoes
        /// <summary>
        /// Adição de uma Instituição
        /// </summary>
        /// <param name="instituicao">conector da classe instituição</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Instituicoes>> PostInstituicao(Instituicoes instituicao)
        {
            // Verifica se a Localidade existe
            var localidade = await _context.Localidades.FindAsync(instituicao.LocalidadeFK);

            // Se a localidade não existir, retorna BadRequest
            if (localidade == null)
            {
                return BadRequest("Localidade não encontrada.");
            }

            // Adiciona a nova instituição à base de dados e salva as alterações
            _context.Instituicoes.Add(instituicao);
            await _context.SaveChangesAsync();

            // Retorna o resultado da criação com o local onde a instituição foi criada
            return CreatedAtAction(nameof(GetInstituicao), new { id = instituicao.Id }, instituicao);
        }

        // PUT: api/Instituicoes/5
        /// <summary>
        /// Endpoint de Atualização de uma Instituição
        /// </summary>
        /// <param name="id">identificador de uma instituição</param>
        /// <param name="instituicao">conector da classe instituição</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstituicao(int id, Instituicoes instituicao)
        {
            // Verifica se o id fornecido corresponde ao id da instituição a ser atualizada
            if (id != instituicao.Id)
            {
                return BadRequest();
            }

            // Confirma se a localidade referenciada existe
            if (!_context.Localidades.Any(l => l.Id == instituicao.LocalidadeFK))
            {
                return BadRequest("Localidade não encontrada.");
            }

            // Marca a instituiçõa como modificada para atualizar os dados
            _context.Entry(instituicao).State = EntityState.Modified;

            // Salva as alterações no contexto da base de dados se não houver erros de concorrência
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstituicaoExists(id))
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

        // DELETE: api/Instituicoes/5
        /// <summary>
        /// Endpoint de Eliminação de uma Instituição
        /// </summary>
        /// <param name="id">identificador de uma instituição</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstituicao(int id)
        {
            // Pesquisa de instituição de acordo com o id fornecido
            var instituicao = await _context.Instituicoes.FindAsync(id);

            // Se não existir instituição com o id fornecido, retorna NotFound
            if (instituicao == null)
            {
                return NotFound();
            }

            // Elimina a instituição encontrada e salva as alterações no contexto da base de dados
            _context.Instituicoes.Remove(instituicao);
            await _context.SaveChangesAsync();

            // Retorna uma resposta de sucesso sem conteúdo
            return NoContent();
        }

        /// <summary>
        /// Verifica se a instituição existe na Base de Dados
        /// </summary>
        /// <param name="id">identificador de uma instituição</param>
        /// <returns></returns>
        private bool InstituicaoExists(int id)
        {
            return _context.Instituicoes.Any(e => e.Id == id);
        }
    }
}
