using IPT_Teste.Data;
using IPT_Teste.Models;
using IPT_Teste.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApplication1;


namespace WebApplication1.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class SignalRController: Controller
{
    private readonly IHubContext<BlocoHubController> _hubContext;
    private readonly ApplicationDbContext _context;
    
    public SignalRController(IHubContext<BlocoHubController> hubContext, ApplicationDbContext context)
    {
        _hubContext = hubContext;
        _context = context;
    }

    [HttpPost("bloco")]
    public async Task<IActionResult>Bloco([FromBody] int turmaId)
    {
        /*
         * id
         * ano
         * letra
         * semestre
         * curso
         */
        
        /*
         * horario -> aulas
         * aulas -> blocos
         * blocos -> blocoshorarios
         * blocoshorarios -> horarios
         */

        //var teste = _context.Blocos.Include(h => h.Horarios).ToList();
        
        
        var aulas = await _context.Aulas.Where(a => a.TurmaFK == turmaId).ToListAsync();


        var aulaIds = aulas.Select(a => a.Id).ToList();

        var blocos = await _context.Blocos
            .Where(b => aulaIds.Contains(b.AulaFK))
            .ToListAsync();

        var horarios = await _context.Horarios
            .Include(h => h.Blocos)
            .ToListAsync();

        var resultado = await _context.Horarios
            .Where(h => h.Blocos.Any(b => aulas.Select(a => a.Id).Contains(b.AulaFK)))
            .Select(h => h.Id)
            .FirstOrDefaultAsync();
        

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", resultado);
        return Ok(resultado);
    }
    
    [HttpPost("horariobloco")]
    public async Task<IActionResult> Horariobloco([FromBody] BlocoDTO dto)
    {
        var horario = await _context.Horarios
            .Include(h => h.Blocos)
            .FirstOrDefaultAsync(h => h.Id == dto.HorarioId);

        if (horario == null)
            return NotFound("Horário não encontrado.");
        
        var conflito = horario.Blocos.Any(b => b.Hora_Inicio == dto.HoraInicio);
        if (conflito)
            return Conflict("Já existe um bloco nessa hora neste horário.");

        var bloco = new Blocos
        {
            Hora_Inicio = dto.HoraInicio,
            SalaFK = dto.SalaFK,
            AulaFK = dto.AulaFK
        };

        _context.Blocos.Add(bloco);
        await _context.SaveChangesAsync(); 

        horario.Blocos.Add(bloco);
        await _context.SaveChangesAsync();

        return Ok("Bloco criado e associado ao horário com sucesso.");
    }
    
    
}