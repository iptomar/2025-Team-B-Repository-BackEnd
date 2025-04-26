using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    public DbSet<Curso> Curso { get; set; }

    public DbSet<Curso_Cadeira> Curso_Cadeira { get; set; }

    public DbSet<Cadeira> Cadeira { get; set; }

    public DbSet<Grau> Grau { get; set; }

    public DbSet<Curso_Instituicao> Curso_Instituicao { get; set; }

    public DbSet<Instituicao> Instituicao { get; set; }

    public DbSet<Tipologia> Tipologia { get; set; }
    
    public DbSet<Turma> Turma { get; set; }

    public DbSet<Sala> Sala { get; set; }

    public DbSet<Localidade> Localidade { get; set; }

    public DbSet<Professor> Professor { get; set; }

    public DbSet<Professor_Cadeira> Professor_Cadeira { get; set; }

    public DbSet<Utilizador> Utilizadador { get; set; }

    public DbSet<IdentityUser> UserManager { get; set; }

    public DbSet<IdentityRole> RoleManager { get; set; }

}
