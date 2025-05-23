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
}