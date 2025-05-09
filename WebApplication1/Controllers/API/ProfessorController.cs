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
        */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Professor> data = await _context.Professor.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint para pesquisar um professor
        * @param Nome - Nome do Professor
        */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Ind(string Nome)
        {
            Professor registo = await _context.Professor.FindAsync(Nome);
            return Ok(registo);
        }

        /**
        * Endpoint para inserir um professor
        */
        [HttpPost("professor")]
        public async Task<IActionResult> Insert([FromBody] Professor professor)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            await _context.Professor.AddAsync(professor);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = professor.Id_professor }, professor);
        }

        /**
        * Endpoint para atualizar um professor
        * @param Id - Id do Professor
        */
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Professor professor)
        {
            _context.Professor.Update(professor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
        * Endpoint para eliminar um professor
        * @param Id - Id do Professor
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Professor registo = await _context.Professor.FindAsync(id);
            _context.Professor.Remove(registo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Endpoint para listar o horário do professor
        /// </summary>
        /// <returns></returns>
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
