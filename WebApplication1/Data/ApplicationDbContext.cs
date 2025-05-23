using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

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

    public DbSet<Cadeira> Cadeira { get; set; }

    public DbSet<Grau> Grau { get; set; }

    public DbSet<Instituicao> Instituicao { get; set; }
    
    public DbSet<Sala> Sala { get; set; }
    
    public DbSet<Localidade> Localidade { get; set; }
    
    public DbSet<Tipologia> Tipologia { get; set; }
    
    public DbSet<Bloco> Bloco { get; set; }
}
