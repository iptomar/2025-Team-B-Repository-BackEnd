using IPT_Teste.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IPT_Teste.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }
    public DbSet<Tipologias> Tipologias { get; set; }
    public DbSet<Cadeiras> Cadeiras { get; set; }
    public DbSet<Graus> Graus { get; set; }
    public DbSet<Instituicoes> Instituicoes { get; set; }
    public DbSet<Localidades> Localidades { get; set; }
    public DbSet<AnosLetivos> AnosLetivos { get; set; }
    public DbSet<Cursos> Cursos { get; set; }
    public DbSet<Salas> Salas { get; set; }
    public DbSet<Turmas> Turmas { get; set; }
    public DbSet<Aulas> Aulas { get; set; }

    public DbSet<Blocos> Blocos { get; set; }

    public DbSet<Horarios> Horarios { get; set; }

    public DbSet<Professores> Professores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cursos>()
            .HasMany(c => c.Cadeiras)
            .WithMany(ca => ca.Cursos)
            .UsingEntity(j => j.ToTable("CursosCadeiras"));

        modelBuilder.Entity<Aulas>()
                .HasOne(a => a.Turma)
                .WithMany()
                .HasForeignKey(a => a.TurmaFK)
                .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Blocos>()
                .HasMany(b => b.Horarios)
                .WithMany(h => h.Blocos)
                .UsingEntity(j => j.ToTable("BlocosHorarios"));
    }
}
