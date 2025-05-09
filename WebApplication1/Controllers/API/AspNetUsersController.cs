using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApplication1.Data;
using WebApplication1.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspNetUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AspNetUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /**
         * Endpoint destinado à Selecao do Nome do Utilizador
         */
        [HttpGet("SelectUserName")]
        public async Task<IActionResult> SelectUserName()
        {
            /**
             * Funcionamento:
             *  - Seleciona o nome do utilizador
             *  - Através da Tabela AspNetUsers
             */
            var nome = _context.UserManager.
                Select(u => u.UserName)
                .ToListAsync();

            return Ok(nome);
        }

        // https://localhost:7110/api/AspNetUsers/SelectNameProfessor
        /**
        * Endpoint destinado à Selecao da Lista de Professores Coordenadores
        */
        [HttpGet("SelectNameProfessor")]
        public async Task<IActionResult> SelectUserNameProfessorCoordenador()
        {
            /**
             * Funcionamento:
             *  - Seleciona o nome do utilizador
             *  - Através da Junção das Tabelas AspNetUsers, AspNetUserRoles, AspNetRoles
             *  - Onde o RoleId seja igual "c0f0653e-b870-4c0a-811f-7fe82f0e8755" 
             */
            var nome = await _context.UserManager
                .Join(_context.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new { user, userRole })
                .Join(_context.Roles,
                    combined => combined.userRole.RoleId,
                    role => role.Id,
                    (combined, role) => new
                    {
                        UserName = combined.user.UserName,
                        RoleId = role.Id,
                        RoleName = role.Name
                    })
                .Where(x => x.RoleId == "c0f0653e-b870-4c0a-811f-7fe82f0e8755")
                .Select(x => new
                {
                    ProfessorCoordenador = x.UserName
                })
                .ToListAsync();

            return Ok(nome);
        }

        /// <summary>
        /// Endpoint que Lista de Utilizadores da aplicação 
        /// (Recebe-se o utilizador atual para devolver todos exceto ele próprio)
        /// </summary>
        /// <returns></returns>
        [HttpGet("Lista_Utilizador")]
        public async Task<IActionResult> ListaUtilizadores()
        {
            var username = User.Identity?.Name;

            if (username == null)
            {
                return Unauthorized();
            }

            // Obter ID do utilizador atual
            var userAtual = await _context.Users
                                .Where(u => u.UserName == username)
                                .Select(u => u.Id)
                                .FirstOrDefaultAsync();

            // Listar todos os utilizadores exceto o atual
            var result = await _context.Users
                                .Where(u => u.Id != userAtual)
                                .Select(u => new
                                {
                                    Nome_Utilizador = u.UserName,
                                    Email_Utilizador = u.Email
                                })
                                .ToListAsync();
            return Ok(result);
        }

    }
}
