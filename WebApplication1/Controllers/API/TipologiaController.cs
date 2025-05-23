using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Controllers.API;

[Route("api/[controller]/[action]")]
[ApiController]
public class TipologiaController : Controller
{
    private readonly ApplicationDbContext _context; 

    public TipologiaController(ApplicationDbContext context)
    {
        _context = context;
    }
    

    [HttpGet]
    public IActionResult Read()
    {
        return Ok(_context.Tipologia.ToList());
    }
}