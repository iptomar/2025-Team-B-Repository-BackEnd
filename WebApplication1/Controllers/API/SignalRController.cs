using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Data;

namespace WebApplication1.Controllers.API;

[Route("api/[controller]/[action]")]
[ApiController]
public class SignalRController : Hub
{
    private readonly ApplicationDbContext _context; 

    public SignalRController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpPost]
    public async Task Send([FromBody] string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}