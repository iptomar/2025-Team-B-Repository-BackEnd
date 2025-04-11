using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Controllers.API;

[Route("api/[controller]/[action]")]
[ApiController]
public class EmailController : Controller
{
    [HttpPost]
    public IActionResult Index([FromBody]EmailCredentialsDTO email)
    {
        //eventualmente, passar isto para algo como um .env
        const int port = 587;
        const string host = "smtp.mailersend.net";
        const string mail = "MS_SWI6e2@test-z0vklo6e38el7qrx.mlsender.net";
        const string password = "mssp.lfbdPMJ.3zxk54vyr7q4jy6v.wvp9E0z";
        
        var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(mail, password),
            EnableSsl = true
        };
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(mail, "IPT"),
            Subject = email.subject,
            Body = email.body,
        };
        
        mailMessage.To.Add(new MailAddress(email.to));

        try
        {
            client.Send(mailMessage);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok("Email enviado com sucesso!");
    }
}