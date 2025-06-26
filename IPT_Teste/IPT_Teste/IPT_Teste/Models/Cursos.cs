using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IPT_Teste.Models
{
    public class Cursos
    {
        [Key]
        public int Id { get; set; }

        public required string Curso { get; set; }

        public int AnoLetivoFK { get; set; }
        [ForeignKey("AnoLetivoFK")]
        public AnosLetivos? AnoLetivo { get; set; }

        public int InstituicaoFK { get; set; }
        [ForeignKey("InstituicaoFK")]
        public Instituicoes? Instituicao { get; set; }

        public int GrauFK { get; set; }
        [ForeignKey("GrauFK")]
        public Graus? Grau { get; set; }

        public required string ProfessorFK { get; set; }
        [ForeignKey("ProfessorFK")]
        public IdentityUser? Professor { get; set; }

        [JsonIgnore]
        public ICollection<Cadeiras> Cadeiras { get; set; } = new List<Cadeiras>();

        [JsonIgnore]
        public ICollection<Professores> Professores { get; set; } = new List<Professores>();
    }
}
