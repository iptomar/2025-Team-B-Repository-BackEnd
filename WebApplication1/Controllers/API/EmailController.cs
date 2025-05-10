using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Controllers.API;

[Route("api/[controller]/[action]")]
[ApiController]
public class EmailController : Controller
{
    
    private readonly ApplicationDbContext _context; 

    public EmailController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    
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

    [HttpPost]
    public IActionResult Recusado([FromBody]EmailCredentialsDTO email, UtilizadorDTO utilizador)
    {
        const int port = 587;
        const string host = "smtp.mailersend.net";
        const string mail = "MS_SWI6e2@test-z0vklo6e38el7qrx.mlsender.net";
        const string password = "mssp.lfbdPMJ.3zxk54vyr7q4jy6v.wvp9E0z";
        
        var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(mail, password),
            EnableSsl = true
        };

        var message = _context.Registo.
            Where(u => u.Utilizador == utilizador.Id_utilizador ).
            Select(r => r.Motivo).
            FirstOrDefault();

        var corpo_message = "O horário foi recusado pelo seguinte motivo:\n" + message;
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(mail, "IPT"),
            Subject = email.subject,
            Body = corpo_message
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
    [HttpPost]
    public IActionResult ContaBloqueada([FromBody] EmailCredentialsDTO email)
    {
        const int port = 587;
        const string host = "smtp.mailersend.net";
        const string mail = "MS_SWI6e2@test-z0vklo6e38el7qrx.mlsender.net";
        const string password = "mssp.lfbdPMJ.3zxk54vyr7q4jy6v.wvp9E0z";
        
        var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(mail, password),
            EnableSsl = true
        };
        
        var corpo_message = "A sua conta foi bloqueada!";
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(mail, "IPT"),
            Subject = email.subject,
            Body = corpo_message
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
    
    [HttpPost]
    public IActionResult Welcome([FromBody] EmailCredentialsDTO email)
    {
        const int port = 587;
        const string host = "smtp.mailersend.net";
        const string mail = "MS_SWI6e2@test-z0vklo6e38el7qrx.mlsender.net";
        const string password = "mssp.lfbdPMJ.3zxk54vyr7q4jy6v.wvp9E0z";
        
        var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(mail, password),
            EnableSsl = true
        };
        
        var corpo_message = "Bom ano letivo!";
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(mail, "IPT"),
            Subject = email.subject,
            Body = corpo_message
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