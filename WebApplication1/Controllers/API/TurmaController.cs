using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurmaController : ControllerBase
    {
        // Referência à BD
        private readonly ApplicationDbContext _context;

        /*
         * Construtor Parametrizado
         * 
         * @param context - Contexto da BD
         */
        public TurmaController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Endpoint de Seleção Genérica das Turmas 
         */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Turma> data = await _context.Turma.ToListAsync();
            return Ok(data);
        }

        /**
         * Endpoint destinado à Seleção de Turmas segundo o Nome do Curso e respetivo Ano Académico.
         * 
         * @param Nome - Nome do Curso
         * @param Ano - Ano Académico do Curso
         */
        [HttpGet("{Nome}/{Ano}")]
        public async Task<IActionResult> Select_Turmas(string Nome, int Ano)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Fetch da(s) Turma(s) segundo o Nome do Curso e Ano Académico da Turma
            var registo = await (from curs in _context.Curso
                                 join turm in _context.Turma on curs.Id_curso equals turm.Curso
                                 where curs.Nome.Contains(Nome) && turm.Ano_academico == Ano
                                 select new
                                 {
                                     Curso = curs.Nome,
                                     Turma = turm.Letra_turma,
                                     Ano = turm.Ano_academico,
                                     Semestre = turm.Semestre
                                 }).ToListAsync();
            // Caso em que não foi encontrada nenhuma turma segundo os parâmetros recebidos
            if (registo == null || !registo.Any())
            {
                return NotFound("Nenhuma turma encontrada para os critérios informados.");
            }
            // Caso em que foi encontrada pelo menos uma turma segundo os parâmetros recebidos 
            return Ok(registo);
        }

        /**
         * Endpoint destinado à inserção de uma nova turma segundo o Nome do Curso
         * 
         * @param Curso - Nome do Curso 
         */
        [HttpPost("{Curso}")]
        public async Task<IActionResult> Insert([FromBody] Turma turma, string Curso)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Fetch do ID do Curso segundo o Nome fornecido
            var curso = await (from curs in _context.Curso
                                where curs.Nome == Curso
                                select new
                                {
                                    ID = curs.Id_curso
                                }).ToListAsync();
            // Caso em que não foi encontrado o ID do Curso segundo o parâmetro recebido
            if (curso == null || !curso.Any())
            {
                return NotFound("Nenhum curso encontrado para o critério fornecido");
            }
            // Caso em que foi encontrado o Curso correspondente
            turma.Curso = curso.First().ID;
            await _context.Turma.AddAsync(turma);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = turma.Curso }, turma);
        }

        /**
         * Endpoint de Edição Genérica das Turmas
         */
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Turma turma)
        {
            _context.Turma.Update(turma);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
         * Endpoint de Apagamento Génerico das Turmas
         */
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            Turma turma = await _context.Turma.FindAsync(id);
            _context.Turma.Remove(turma);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
