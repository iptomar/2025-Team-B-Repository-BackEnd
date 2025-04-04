using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admistrador", "Docente", "Coordenador de Curso", "Comissão de Horários", "Comissão de Cursos", "Diretor/a" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
