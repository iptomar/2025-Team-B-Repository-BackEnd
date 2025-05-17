using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioController : Controller
    {
        // Referência à BD
        private readonly ApplicationDbContext _context;

        /*
         * Construtor Parametrizado
         * 
         * @param context - Contexto da BD
         */
        public HorarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Endpoint de Seleção Genérica dos Horários
         * Estado: ✓
         */
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            List<Horario> data = await _context.Horario.ToListAsync();
            return Ok(data);
        }

        /**
        * Endpoint destinado à Seleção de Horários segundo o Nome do Curso
        * Estado: ✓
        * 
        * @param Nome - Nome do Curso
        */
        [HttpGet("{Nome}")]
        public async Task<IActionResult> Select_Horario(string Nome)
        {
            // Pesquisa do ID do Curso 
            var registo = await (from cursos in _context.Curso
                                 where cursos.Nome == Nome
                                 select new
                                 {
                                     Id_curso = cursos.Id_curso,
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrado nenhum Curso segundo os parâmetros recebidos
            if (registo == null) {
                return NotFound("Nenhum curso encontrado para o critério fornecido.");
            // Caso em que foi encontrado pelo menos um Curso segundo os parâmetros recebidos
            } else {
                // Pesquisa do Horário segundo o ID do Curso
                var horario = await (from hor in _context.Horario
                                     where hor.Curso == registo.Id_curso
                                     select new
                                     {
                                         Id_horario = hor.Id_horario,
                                         Data_inicio = hor.Data_inicio,
                                         Data_fim = hor.Data_fim
                                     }).FirstOrDefaultAsync();
                // Caso em que o Curso ainda não possui Horários
                if (horario == null) {
                    return NotFound("Ainda não existem Horários para o Curso em questão.");
                } else {
                // Caso em que o Curso possui, pelo menos, um Horário
                    return Ok(horario);
                }
            }
        }

        /**
        * Endpoint de Inserção de Horários
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Horario horario)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verificação se o Horário a ser inserido já existe
            var registo = await (from hor in _context.Horario
                                 where hor.Curso == horario.Curso && hor.Data_fim == horario.Data_inicio && hor.Data_fim == horario.Data_fim
                                 select new
                                 {
                                     Id_horario = hor.Id_horario
                                 }).FirstOrDefaultAsync();
            // Caso em que não foi encontrado nenhum Horário com as mesmas credênciais
            if (registo == null)
            {
                await _context.Horario.AddAsync(horario);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = horario.Id_horario }, horario);
            }
            // Caso em que foi encontrado pelo menos um Horário com as mesmas credênciais
            return NotFound("O Horário que deseja inserir já se encontra na BD.");
        }

        /**
        * Endpoint para Atualizar os Horários
        * Estado: ✘
        * Nota: Este ainda não dá devido a serem necessárias alterações ao Model.
        */
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Horario horario)
        {
            // Verificação se o modelo fornecido é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Pesquisa do Horário pelo seu ID 
            var registo = await (from hor in _context.Horario
                                 where hor.Id_horario == horario.Id_horario
                                 select new
                                 {
                                     Id_Horario = hor.Id_horario
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrado o Horário definido
            if (registo != null)
            {
                _context.Horario.Update(horario);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrado o Horário definido
            return NotFound("O Horário que especificou não existe na BD.");
        }

        /**
         * Endpoint de Apagamento dos Horários
         * Estado: ✘
         * Nota: Este vai funcionar mas pode apagar mais do que uma não existe grau de diferenciação nas turmas. 
         * 
         * @param Nome - Nome do Curso
         * @param Nome_grau - Nome do Grau
         */
        [HttpDelete("{Nome}/{Nome_grau}")]
        public async Task<IActionResult> Delete(string Nome, string Nome_grau)
        {
            // Pesquisa do Horário pelo Nome do Curso e Nome do Grau 
            var registo = await (from gr in _context.Grau
                                 join cur in _context.Curso on gr.Id_grau equals cur.Grau
                                 join hor in _context.Horario on cur.Id_curso equals hor.Curso
                                 where gr.Nome_grau == Nome_grau && cur.Nome == Nome
                                 select new
                                 {
                                     Id_horario = hor.Id_horario
                                 }).FirstOrDefaultAsync();
            // Caso em que foi encontrado o Horário definido
            if (registo != null)
            {
                // Encontra o Horário pelo o seu ID
                Horario horario = await _context.Horario.FindAsync(registo.Id_horario);
                // Apaga o mesmo e salva as alterações na BD
                _context.Horario.Remove(horario);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Caso em que não foi encontrado o Horário definido
            return NotFound("O Horário que especificou não existe na BD.");
        }
    }
}
