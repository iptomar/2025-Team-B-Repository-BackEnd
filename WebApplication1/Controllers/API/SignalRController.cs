using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

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
    public async Task<IActionResult>Bloco([FromBody] string salaAula, string tipologiaCadeira, string nomeCadeira, DateTime inicio)
    {
        var ano = DateTime.Now.Year.ToString();
        
        //partindo do principio que o frontend envia strings como argumentos, fez-se a passagem desses valores para um int (tabela Bloco so aceita esses valores como inteiro (FKs))
        
        //sala --> int
        var s = _context.Sala.  
            Where(sala => sala.Nome_sala == salaAula).
            Select(sa => sa.Id_sala). 
            FirstOrDefault();
        
        //tipologia --> int
        var t = _context.Tipologia. 
            Where(tipologia => tipologia.Nome_tipologia == tipologiaCadeira ). 
            Select(ti => ti.Id_tipologia). 
            FirstOrDefault();
        
        //cadeira --> int
        var c = _context.Cadeira. 
            Where(cadeira => cadeira.Nome_cadeira == nomeCadeira). 
            Select(cadeira => cadeira.Id_cadeira).
            FirstOrDefault();
            
        var bloco = new Bloco
        {
            //é necessario resolver questões nos Models
        };
        
        _context.Bloco.Add(bloco);
        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync("", bloco);

        return Ok(bloco);
    }
    
    
}