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
    public async Task<IActionResult>Bloco([FromBody] TurmaDTO turma)
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
        
        
        var horario =  _context.Aulas. 
            Where(a => a.Id == turma.Id ). 
            SelectMany(aula => _context.Blocos. 
                Where(b => b.AulaFK == aula.Id ). 
                SelectMany( bloco => _context.Horarios.Include(h => h.Blocos). 
                    Where(h => h.Blocos == bloco.Horarios). 
                    Select(horario => horario.Id))
            ).FirstOrDefault();
        
        
        /*var horario = await _context.Aulas
            .Where(a => a.TurmaFK == turma.Id)
            .SelectMany(aula => _context.Blocos
                .Where(b => b.AulaFK == aula.Id)
                .SelectMany(bloco => _context.Set<Dictionary<string, object>>("blocoshorarios")
                    .Where(bh => (int)bh["BlocosId"] == bloco.Id)
                    .Join(_context.Horarios,
                        bh => (int)bh["HorariosId"],
                        h => h.Id,
                        (bh, horario) => new {
                            Cadeira = aula.CadeiraFK,
                            Sala = bloco.SalaFK,
                            HoraInicio = horario.Inicio,
                            HoraFim = horario.Fim
                        }
                    )
                )
            )
            .ToListAsync();*/
        

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", horario);
        return Ok(horario);
    }
    
    
}