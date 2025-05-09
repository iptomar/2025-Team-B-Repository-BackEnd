using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            string role = model.Role ?? "Docente";

            var novoUtilizador = new Utilizador();

            novoUtilizador.IDAspNetUser = user.Id;
            novoUtilizador.Nome = user.UserName;


            if (!await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            await _context.AddAsync(novoUtilizador);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Utilizador criado com sucesso com papel de {role}" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized("Email não encontrado.");

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
            if (!result.Succeeded) return Unauthorized("Credenciais Inválidas");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        /// <summary>
        /// Endpoint para receber o JWT Token e decifrar (Devolver o ID do utilizador) 
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [HttpGet("get-user-id")]
        public IActionResult GetUserIdFromToken()
        {
            // Verifica se está autenticado
            if (!User.Identity.IsAuthenticated)
                return Unauthorized("Token inválido ou expirado.");

            // Tenta obter o ID do utilizador do claim "sub"
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return BadRequest("O token não contém o ID do utilizador.");

            return Ok(new { UserId = userId });
        }


        private string GenerateJwtToken(IdentityUser user)
        {
            var userRoles = _userManager.GetRolesAsync(user).Result;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}