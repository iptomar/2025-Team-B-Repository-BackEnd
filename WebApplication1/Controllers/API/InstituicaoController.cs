using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstituicaoController : ControllerBase
    {
        // Referência à BD
        private readonly ApplicationDbContext _context;

        /*
        * Construtor Parametrizado
        * 
        * @param context - Contexto da BD
        */
        public InstituicaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Endpoint de Seleção Genérica das Instituições
         * Estado: ✓
         */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Instituicao> data = await _context.Instituicao.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint destinado à Seleção de Instituição segundo o seu Nome
        * Estado: ✓
        * 
        * @param Nome_instituicao - Nome da Instituição
        */
        /*
        [HttpGet("{Nome_instituicao}")]
        public async Task<IActionResult> Select_Ind(string Nome)
        {
            Instituicao registo = await _context.Instituicao.FindAsync(Nome);
            return Ok(registo);
        }*/

        /*
         * Endpoint destinado à Pesquisa dos Cursos disponíveis por Instituição
         * Estado: ✓
         * 
         * @param Nome_instituicao - Nome da Institução
         */
        [HttpGet("{Nome_instituicao}")]
        public async Task<IActionResult> SelectInstituicao_Curso(string Nome_instituicao)
        {
            // Pesquisa de Cursos de uma Instituição através do Nome da mesma
            var result = await _context.Instituicao
                .Join(_context.Curso_Instituicao,
                    instituicao => instituicao.Id_instituicao,
                    cinstituicao => cinstituicao.Id_instituicao,
                    (instituicao, cinstituicao) => new { instituicao, cinstituicao })
                .Join(_context.Curso,
                    ci2 => ci2.cinstituicao.Id_curso,
                    curso => curso.Id_curso,
                    (ci2, curso) => new { ci2, curso })
                .Join(_context.Grau,
                    cc2 => cc2.curso.Grau,
                    grau => grau.Id_grau,
                    (cc2, grau) => new
                    {
                        NomeCurso = cc2.curso.Nome,
                        Grau = grau.Nome_grau,
                        NomeInstituicao = cc2.ci2.instituicao.Nome_instituicao
                    })
                .Where(x => x.NomeInstituicao == Nome_instituicao)
                .Select(x => new
                {
                    Nome_Curso = x.NomeCurso,
                    Nome_Grau = x.Grau
                })
                .ToListAsync();
            // Caso em que foram encontrados cursos para a instituição fornecida
            if (result.Count == 0)
            {
                return NotFound("Não foram encontrados cursos para a instituição fornecida.");
            }
            // Caso em que não foi encontrado pelo menos um curso para a instituição fornecida
            return Ok(result);
        }


        /**
        * Endpoint de Inserção de Instituições
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPost("instituicao")]
        public async Task<IActionResult> Insert([FromBody] Instituicao instituicao)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState); 
            }
            // Verificação se a Instituição a ser inserida já existe
            var registo = await (from ins in _context.Instituicao
                                 where ins.Nome_instituicao == instituicao.Nome_instituicao
                                 select new
                                 {
                                     Id = ins.Id_instituicao
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrado nenhuma Instituição com o mesmo nome
            if (registo == null)
            {
                await _context.Instituicao.AddAsync(instituicao);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = instituicao.Id_instituicao }, instituicao);
            }
            // Caso em que foi encontrado pelo menos uma Instituição com o mesmo nome
            return NotFound("A Instituição que deseja inserir já se encontra na BD.");
        }

        /**
        * Endpoint para Atualizar as Instituições
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Instituicao instituicao)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa da Instituição pelo seu Nome
            var registo = await (from ins in _context.Instituicao
                                 where ins.Id_instituicao == ins.Id_instituicao
                                 select new
                                 {
                                     Id_instituicao = ins.Id_instituicao
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrada a instituição definida
            if (registo != null)
            {
                _context.Instituicao.Update(instituicao);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrada a instituição definida
            return NotFound("A instituição que especificou não existe na BD.");
        }

        /**
        * Endpoint de Apagamento dos Instituições
        * Estado: ✓
        * 
        * @param Nome_instituicao - Nome da Instituição
        */
        [HttpDelete("{Nome_instituicao}")]
        public async Task<IActionResult> Delete(string Nome_instituicao)
        {
            // Pesquisa do Grau pelo seu Nome
            var registo = await (from ins in _context.Instituicao
                                 where ins.Nome_instituicao == Nome_instituicao
                                 select new
                                 {
                                     Id_instituicao = ins.Id_instituicao
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrado a instituição definida
            if (registo != null)
            {
                // Encontra a Instituição pelo o seu ID
                Instituicao instituicao = await _context.Instituicao.FindAsync(registo.Id_instituicao);
                // Apaga o mesmo e salva as alterações na BD
                _context.Instituicao.Remove(instituicao);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrada a instituição definida
            return NotFound("O grau que especificou não existe na BD.");
        }
        
        /**
         * Endpoint destinado à Selecao do Nome da Instituição
         */
        /*
        [HttpGet("SelectNomeInstituicao")]
        public async Task<IActionResult> SelectNomeInstituicao()
        {
            var nome = _context.Instituicao.
                Select(i => i.Nome_instituicao)
                .ToListAsync();

            return Ok(nome);
        }*/
    }
}
