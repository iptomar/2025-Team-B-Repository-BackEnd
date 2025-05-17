using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        // Referência à BD
        private readonly ApplicationDbContext _context;

        /*
        * Construtor Parametrizado
        * 
        * @param context - Contexto da BD
        */
        public CursoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Endpoint de Seleção Genérica de Cursos
         * Estado: ✓
         */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Curso> data = await _context.Curso.ToListAsync();
            return Ok(data);
        }

        /**
         * Endpoint destinado à Seleção de Cursos segundo o seu Nome
         * Estado: ✓
         * 
         * @param Nome - Nome do Curso
         */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Cur(string Nome)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do Curso segundo o seu Nome 
            var registo = await (from cur in _context.Curso
                                 join usr in _context.UserManager on cur.Coordenador equals usr.Id
                                 where cur.Nome == Nome
                                 select new
                                 {
                                     Id_curso = cur.Id_curso,
                                     Nome = cur.Nome,
                                     Ano = cur.Ano,
                                     Coordenador = usr.NormalizedUserName
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrada nenhum Curso segundo os parâmetros recebidos
            if (registo == null)
            {
                return NotFound("Nenhum curso encontrado para o critério fornecido.");
            }
            // Caso em que foi encontrado pelo menos um Grau segundo os parâmetros recebidos 
            return Ok(registo);
        }

        /**
        * Endpoint de Inserção de Cursos
        * Estado: ✘ ?
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPost("curso")]
        public async Task<IActionResult> Insert([FromBody] Curso curso)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o Curso a ser inserido já existe
            var registo = await (from cur in _context.Curso
                                 where cur.Nome == curso.Nome && cur.Ano == curso.Ano && cur.Grau == curso.Grau
                                 select new
                                 {
                                     Id_curso = cur.Id_curso
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrado nenhum Curso com o mesmo nome, ano e grau
            if (registo == null)
            {
                await _context.Curso.AddAsync(curso);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = curso.Id_curso }, curso);
            }
            // Caso em que foi encontrado pelo menos um Curso com o mesmo nome, ano e grau
            return NotFound("O Curso que deseja inserir já se encontra na BD.");
        }

        /**
        * Endpoint para Atualizar os Cursos
        * Estado: ✘ ?
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Curso curso)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do Curso pelo seu ID 
            var registo = await (from cur in _context.Curso
                                 where cur.Id_curso == curso.Id_curso
                                 select new
                                 {
                                     Id_curso = cur.Id_curso
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrado o curso definido
            if (registo != null)
            {
                _context.Curso.Update(curso);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrado o curso definido
            return NotFound("O curso que especificou não existe na BD.");
        }

        /**
         * Endpoint de Apagamento dos Cursos
         * Estado: ?
         * 
         * @param Nome - Nome do Curso
         * @param Ano - Ano do Curso
         * @param Grau - Nome do Grau
         */
        [HttpDelete("{Nome_curso/Ano/Grau}")]
        public async Task<IActionResult> Delete(string Nome, string Ano, string Grau)
        {
            // Pesquisa do Curso pelo seu nome, ano e grau
            var registo = await (from gra in _context.Grau
                                 join cur in _context.Curso on gra.Id_grau equals cur.Grau
                                 where cur.Nome == Nome && cur.Ano == Ano && gra.Nome_grau == Grau
                                 select new
                                 {
                                     Id_curso = cur.Id_curso
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrado o Curso definido
            if (registo != null)
            {
                // Encontra o Curso pelo o seu ID
                Curso curso = await _context.Curso.FindAsync(registo.Id_curso);
                // Apaga o mesmo e salva as alterações na BD
                _context.Curso.Remove(curso);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrado o grau definido
            return NotFound("O curso que especificou não existe na BD.");
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
        [HttpGet("{ano}/ {nome}")]
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
                            IdCurso = pc2.ccadeira.cc2.curso.Id_curso,
                            NomeProfessor = professor.Nome

                        })
                    .Where(x => x.AnoCurso == ano && x.NomeCurso == nome)
                    .Select(x => new
                    {
                        ID = x.IdCurso,
                        Professores = x.NomeProfessor
                    })
                    .ToListAsync();
            return Ok(result);
        }

        /**
        * Endpoint destinado à Pesquisa de Professores lecionadores de dado curso (2)
        */
        [HttpGet("{ano}/ {nome}")]
        public async Task<IActionResult> SelectProfessor_Curso_2(string ano, string nome)
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
                            IdProfessor = professor.Id_professor,
                            NomeProfessor = professor.Nome

                        })
                    .Where(x => x.AnoCurso == ano && x.NomeCurso == nome)
                    .Select(x => new
                    {
                        ID = x.IdProfessor,
                        Professores = x.NomeProfessor
                    })
                    .ToListAsync();
            return Ok(result);
        }

        /**
        * Endpoint destinado à Pesquisa de Professores lecionadores de dado curso e respetivas cadeiras
        */
        [HttpGet("{ano}/ {nome}")]
        public async Task<IActionResult> SelectProfessor_Curso_Cadeiras(string ano, string nome)
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
                            NomeCadeira = pc2.ccadeira.cadeira.Nome_cadeira,
                            NomeProfessor = professor.Nome

                        })
                    .Where(x => x.AnoCurso == ano && x.NomeCurso == nome)
                    .Select(x => new
                    {
                        Nome_Curso = x.NomeCurso,
                        Nome_Cadeira = x.NomeCadeira,
                        Professores = x.NomeProfessor
                    })
                    .ToListAsync();
            return Ok(result);
        }
    }
}
