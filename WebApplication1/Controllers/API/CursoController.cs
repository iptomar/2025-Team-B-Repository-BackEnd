using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CursoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /**
         * Endpoint da Lista de Cursos
         */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Curso> data = await _context.Curso.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint de pesquisa de um curso
        * @param Nome - Nome do Curso
        */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Ind(string Nome)
        {
            Curso registo = await _context.Curso.FindAsync(Nome);
            return Ok(registo);
        }

        /**
        * Endpoint para inserir um novo curso
        */
        [HttpPost("curso")]
        public async Task<IActionResult> Insert([FromBody] Curso curso)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            await _context.Curso.AddAsync(curso);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = curso.Id_curso }, curso);
        }

        /**
        * Endpoint para atualizar um curso
        * @param Id - Id do Curso
        */
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Curso curso)
        {
            _context.Curso.Update(curso);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
        * Endpoint para eliminar um curso
        * @param Id - Id do Curso
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Curso registo = await _context.Curso.FindAsync(id);
            _context.Curso.Remove(registo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
         * Endpoint destinado à Selecao do Ano do Curso
         */
        [HttpGet("SelectAnoCurso")]
        public async Task<IActionResult> SelectAnoCurso()
        {
            /**
             * Funcionamento:
             *  - Seleciona o ano do curso
             *  - Através da Tabela Curso
             */
            var ano = _context.Curso.
                Select(c => c.Ano)
                .ToListAsync();

            return Ok(ano);
        }

        /**
        * Endpoint destinado à Pesquisa de Professores lecionadores de dado curso
        */
        [HttpGet("Professor/{ano}/{nome}")]
        public async Task<IActionResult> SelectProfessor_Curso(string ano, string nome)
        {
            var result = await _context.Curso
                                .Join(_context.Curso_Cadeira,
                                    curso => curso.Id_curso,
                                    ccurso => ccurso.Id_curso,
                                    (curso, ccurso) => new { curso, ccurso })
                                .Join(_context.Cadeira,
                                    cc2 => cc2.ccurso.Id_cadeira,
                                    cadeira => cadeira.Id_cadeira,
                                    (cc2, cadeira) => new { cc2, cadeira })
                                .Join(_context.Professor_Cadeira,
                                    ccadeira => ccadeira.cadeira.Id_cadeira,
                                    pcadeira => pcadeira.Id_cadeira,
                                    (ccadeira, pcadeira) => new { ccadeira, pcadeira })
                                .Join(_context.Professor,
                                    pc2 => pc2.pcadeira.Id_professor,
                                    professor => professor.Id_professor,
                                    (pc2, professor) => new
                                    {
                                        AnoCurso = pc2.ccadeira.cc2.curso.Ano,
                                        NomeCurso = pc2.ccadeira.cc2.curso.Nome,
                                        IdProf = professor.Id_professor,
                                        NomeProfessor = professor.Nome,
                                        NomeCadeira = pc2.ccadeira.cadeira.Nome_cadeira
                                    })
                                .Where(x => x.AnoCurso == ano && x.NomeCurso == nome)
                                .Select(x => new
                                {
                                    ID = x.IdProf,
                                    Professor = x.NomeProfessor,
                                    NCurso = x.NomeCurso,
                                    NCadeira = x.NomeCadeira
                                })
                                .ToListAsync();
            return Ok(result);
        }
    }
}
