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
         * Estado: ✓
         */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Turma> data = await _context.Turma.ToListAsync();
            return Ok(data);
        }

        /**
         * Endpoint destinado à Seleção de Turmas segundo o Nome do Curso e respetivo Ano Académico.
         * Estado: ✓
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
            // Caso em que não foi encontrada nenhuma Turma segundo os parâmetros recebidos
            if (registo == null || !registo.Any())
            {
                return NotFound("Nenhuma turma encontrada para os critérios fornecidos.");
            }
            // Caso em que foi encontrada pelo menos uma Turma segundo os parâmetros recebidos 
            return Ok(registo);
        }

        /**
         * Endpoint destinado à inserção de uma nova turma segundo o Nome do Curso
         * Estado: ✓
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
            // Pesquisa do ID do Curso segundo o Nome fornecido
            var registo = await (from curs in _context.Curso
                               where curs.Nome == Curso
                               select new
                               {
                                   ID = curs.Id_curso
                               }).ToListAsync();
            // Caso em que não foi encontrado o ID do Curso segundo o parâmetro recebido
            if (registo == null || !registo.Any())
            {
                return NotFound("Nenhum registo encontrado para o critério fornecido");
            }
            // Caso em que foi encontrado o Curso correspondente
            turma.Curso = registo.First().ID;
            await _context.Turma.AddAsync(turma);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = turma.Curso }, turma);
        }

        /**
         * Endpoint de Edição das Turmas
         * Estado: ✘
         * Nota: Este não é possível devido ao facto que três dos atributos pertencem à PK e um é FK.
         */
        [HttpPut("{Letra_turma}/{Ano_academico}/{Semestre}/{Curso}")]
        public async Task<IActionResult> Edit([FromBody] Turma turma, string Letra_turma, int Ano_academico, int Semestre, int Curso)
        {
            // Verificação se o modelo fornecido é válido 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se a Turma existe na BD 
            var turmaCheck = await _context.Turma.FirstOrDefaultAsync(turm =>
               turm.Letra_turma == Letra_turma &&
               turm.Ano_academico == Ano_academico &&
               turm.Semestre == Semestre &&
               turm.Curso == Curso
            );
            // Caso em que foi encontrada a Turma correspondente
            if (turmaCheck != null)
            {
                _context.Entry(turmaCheck).CurrentValues.SetValues(turma);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrada a Turma correspondente
            return NotFound("Nenhuma turma encontrada para os critérios fornecidos.");
        }

        /**
         * Endpoint de Apagamento de Turmas
         * Estado: ✓
         */
        [HttpDelete("{Curso}")]
        public async Task<IActionResult> Delete([FromBody] Turma turma, string Curso)
        {
            // Verificação se o modelo fornecido é válido 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do ID do Curso segundo o Nome fornecido
            var registo = await (from curs in _context.Curso
                               where curs.Nome == Curso
                               select new
                               {
                                   Id_curso = curs.Id_curso
                               }).FirstOrDefaultAsync();
            // Definição do ID do Curso da Turma
            turma.Curso = registo.Id_curso;
            // Verificação se a Turma existe na BD 
            var turmaObj = await _context.Turma.FirstOrDefaultAsync(turm =>
               turm.Letra_turma == turma.Letra_turma &&
               turm.Ano_academico == turma.Ano_academico &&
               turm.Semestre == turma.Semestre &&
               turm.Curso == turma.Curso
            );
            // De-attach do rastreio de turmaObj
            _context.Entry(turmaObj).State = EntityState.Detached;
            // Caso em que foi encontrada a Turma correspondente
            if (turmaObj != null)
            {
                _context.Turma.Remove(turma);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrada a Turma correspondente
            return NotFound("Nenhuma turma encontrada para os critérios informados.");
        }
    }
}
