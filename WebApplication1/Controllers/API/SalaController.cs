using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Controllers.API;

[Route("api/[controller]/[action]")]
[ApiController]
public class SalaController : Controller
{
    private readonly ApplicationDbContext _context; 

    public SalaController(ApplicationDbContext context)
    {
        //injeção da base de dados na var global (este controller depende da ligacao à db)
        _context = context;
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] SalaDTO sala)
    {
       
        if (sala.Nome_sala == "")
        {
            return BadRequest();
        }
        
        var localidade = sala.localidade; //Tomar
        
        //depois de verificar se uma sala esta realmente a ser introduzida, começar com a logica
        //criar um objeto sala e passar-lhe os valores do DTO que são passados por Body
        
        Sala s1 = new Sala();
        /*
         * valores a passar:
         * 1- Id_sala (doesnt really matter what goes in there)
         * 2- Nome_sala
         * 3- Ir buscar o ID de determinada localidade através da string 
         */
        
        var local = _context.Localidade.
            Where(local => local.Nome_localidade == localidade).
            Select(loc => loc.Id_localidade ).
            FirstOrDefault();
        
        s1.Nome_sala = sala.Nome_sala;
        s1.Id_sala = 0;
        s1.localidade = local;
        
        _context.Sala.Add(s1);
        _context.SaveChanges();
        return Created("", s1);

    }
    
    [HttpDelete]
    public IActionResult Delete([FromQuery] SalaDTO sala) 
    {
        //ir buscar o ID da sala
        //a partir desse ID, fazer a remoção da mesma
        
        var idSala = _context.Sala.
            Where(s => sala.Nome_sala == s.Nome_sala).
            Select(s => s.Id_sala).
            FirstOrDefault();
        
        //como se recebe uma string da parte do frontend, ir buscar o numero correspondente à localidade em questão
        var localidade = _context.Localidade.
            Where(l => l.Nome_localidade == sala.localidade).
            Select(l => l.Id_localidade).
            FirstOrDefault();

        Sala s1 = new Sala(); //Id_sala, localidade, Nome_sala
        s1.Id_sala = idSala;
        s1.localidade = localidade;
        s1.Nome_sala = sala.Nome_sala;

        if (s1.Id_sala == 0 || s1.localidade == 0)
        {
            return BadRequest("Nenhuma sala com o nome indicado foi encontrada");
        }
        
        _context.Sala.Remove(s1);
        _context.SaveChanges();
        
        return Ok("A sala " + sala.Nome_sala + " foi removida com sucesso!");
    }
    
    [HttpPut]
    public IActionResult Update([FromBody] SalaDTO sala)
    {
        /*
         * 1 - fazer a consulta na db
         * 2 - inserir no DTO (suporta ter uma string na localidade)
         */

        var checkSala = _context.Sala.First(s => s.Nome_sala == sala.Nome_sala);

        if (checkSala == null)
        {
            return BadRequest("Nenhuma sala com o nome indicado foi encontrada.");
        }
        
        var localidade = _context.Localidade.
            Where(l => l.Nome_localidade == sala.localidade).
            Select(l => l.Id_localidade).
            FirstOrDefault();
        
        checkSala.Nome_sala = sala.Nome_sala;
        checkSala.localidade = localidade;
        _context.Sala.Update(checkSala);
        _context.SaveChanges();
        
        return Ok("A sala foi alterada com sucesso!");
    }

    [HttpGet]
    public IActionResult Read()
    {
        return Ok(_context.Sala.ToList());
    }
}