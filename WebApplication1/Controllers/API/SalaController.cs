using System.Runtime.Intrinsics.Arm;
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
       
        if (sala.Nome_sala == null)
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
    public IActionResult Delete()
    {
        return Ok();
    }
    
    [HttpPut]
    public IActionResult Update()
    {
        return Ok();
    }

    [HttpGet]
    //[Authorize]
    public IActionResult Read()
    {
        return Ok(_context.Sala.ToList());
    }
}