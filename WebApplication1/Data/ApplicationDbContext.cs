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

    public DbSet<Cadeira> Cadeira { get; set; }

    public DbSet<Grau> Grau { get; set; }

    public DbSet<Instituicao> Instituicao { get; set; }
}
