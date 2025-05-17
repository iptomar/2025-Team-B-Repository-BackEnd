using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        /**
        * Endpoint da Lista de Professores
        * Estado: ✓
        */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Professor> data = await _context.Professor.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint para pesquisar um professor
        * Estado: ✓
        * 
        * @param Nome - Nome do Professor
        */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Prof(string Nome)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do Professor segundo o Nome 
            var registo = await (from prof in _context.Professor
                                 where prof.Nome == Nome
                                 select new
                                 {
                                     Id = prof.Id_professor,
                                     Grau = prof.Nome
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrada nenhum Professor segundo os parâmetros recebidos
            if (registo == null)
            {
                return NotFound("Nenhum professor encontrado para o critério fornecido.");
            }
            // Caso em que foi encontrado pelo menos um Professor segundo os parâmetros recebidos 
            return Ok(registo);
        }

        /**
        * Endpoint para inserir um professor
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPost("professor")]
        public async Task<IActionResult> Insert([FromBody] Professor professor)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o Professor a ser inserido já existe
            var registo = await (from prof in _context.Professor
                                 where prof.Nome == professor.Nome
                                 select new
                                 {
                                     Id = prof.Id_professor
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrado nenhum Professor com o mesmo nome
            if (registo == null)
            {
                await _context.Professor.AddAsync(professor);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = professor.Id_professor }, professor);
            }
            // Caso em que foi encontrado pelo menos um Professor com o mesmo nome
            return NotFound("O Professor que deseja inserir já se encontra na BD.");
        }

        /**
        * Endpoint para atualizar um professor
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Professor professor)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do Professor pelo seu ID 
            var registo = await (from prof in _context.Professor
                                 where prof.Id_professor == professor.Id_professor
                                 select new
                                 {
                                     Id_professor = prof.Id_professor
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrado o professor definido
            if (registo != null)
            {
                _context.Professor.Update(professor);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrado o grau definido
            return NotFound("O professor que especificou não existe na BD.");
        }

        /**
        * Endpoint para eliminar um professor
        * Estado: ✓
        * 
        * @param Id - Id do Professor
        */
        [HttpDelete("{Nome}")]
        public async Task<IActionResult> Delete(string Nome)
        {
            // Pesquisa do Professor pelo seu Nome 
            var registo = await (from prof in _context.Professor
                                 where prof.Nome == Nome
                                 select new
                                 {
                                     Id_professor = prof.Id_professor
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrado o Professor definido
            if (registo != null)
            {
                // Encontra o Professor pelo o seu Nome
                Professor prof = await _context.Professor.FindAsync(registo.Id_professor);
                // Apaga o mesmo e salva as alterações na BD
                _context.Professor.Remove(prof);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrado o Professor definido
            return NotFound("O professor que especificou não existe na BD.");
        }

        /**
        * Endpoint para listar o horário do professor
        * Estado: ?
        * Nota: Funciona mas não possui dados para testagem.
        * 
        * @param nome - Nome do Professor, ano - Ano Letivo do Curso
        */
        [HttpGet("{nome}/{ano}")]
        public async Task<IActionResult> SelectProfessor_Horario(string nome, string ano)
        {
            var result = await _context.Professor
                    .Join(_context.Professor_Cadeira,
                        p => p.Id_professor,
                        pc => pc.Id_professor,
                        (p, pc) => new { p, pc})
                    .Join(_context.Cadeira,
                        pc2 => pc2.pc.Id_cadeira,
                        c => c.Id_cadeira,
                        (pc2, c) => new {pc2, c})
                    .Join(_context.Curso_Cadeira,
                        c2 => c2.c.Id_cadeira,
                        cc => cc.Id_cadeira,
                        (c2, cc) => new {c2, cc})
                    .Join(_context.Curso,
                        cc2 => cc2.cc.Id_curso,
                        crs => crs.Id_curso,
                        (cc2, crs) => new {cc2, crs})
                    .Join(_context.Horario,
                        crs2 => crs2.crs.Id_curso,
                        h => h.Curso,
                        (crs2, h) => new
                        {
                            nomeProfessor = crs2.cc2.c2.pc2.p.Nome,
                            anoLetivo = crs2.crs.Ano,
                            horarioLetivo = h.Id_horario
                        })
                    .Where(x => x.nomeProfessor.ToLower() == nome.ToLower() && x.anoLetivo == ano)
                    .Select(x => new
                    {
                        horarioProfessor = x.horarioLetivo
                    })
                    .ToListAsync();
            return Ok(result);
        }
    }
}
