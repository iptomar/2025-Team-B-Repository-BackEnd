using IPT_Teste.Data;
using IPT_Teste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
    public async Task<IActionResult>Bloco([FromBody] string salaAula, string tipologiaCadeira, string nomeCadeira, TimeOnly inicio)
    {
        
        //partindo do principio que o frontend envia strings como argumentos, fez-se a passagem desses valores para um int (tabela Bloco so aceita esses valores como inteiro (FKs))
        
        //sala --> int
        var s = _context.Salas.  
            Where(sala => sala.Sala == salaAula).
            Select(sa => sa.Id). 
            FirstOrDefault();
        
        //tipologia --> int
        var t = _context.Tipologias. 
            Where(tipologia => tipologia.Tipologia == tipologiaCadeira ). 
            Select(ti => ti.Id). 
            FirstOrDefault();
        
        //cadeira --> int
        var c = _context.Cadeiras. 
            Where(cadeira => cadeira.Cadeira == nomeCadeira). 
            Select(ca => ca.Id).
            FirstOrDefault();

        var a = _context.Aulas.
                Where(aula => aula.CadeiraFK == c) . 
                Select(aa => aa.Id). 
                FirstOrDefault();
            ;
        var bloco = new Blocos
        {
            Hora_Inicio = inicio,
            SalaFK = s,
            AulaFK = a
        };
        
        _context.Blocos.Add(bloco);
        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", bloco);

        return Ok(bloco);
    }
    
    
}