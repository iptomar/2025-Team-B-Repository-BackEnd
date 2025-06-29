using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplication1;

namespace WebApplication1.Controllers.API;

public class BlocoHubController : Hub
{
    
    public async Task Send([FromBody]  object info )
    {
        await Clients.All.SendAsync("ReceiveMessage", info);
    }
    
    public async Task JoinHorarioGroup(int horarioId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"horario_{horarioId}");
    }
    
    public async Task LeaveHorarioGroup(int horarioId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Horario_{horarioId}");
    }
    
    public async Task UpdateBlocoGroup(int blocoId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"bloco_{blocoId}");
    }
}