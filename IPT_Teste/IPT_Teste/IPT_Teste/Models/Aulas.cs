using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IPT_Teste.Models
{
    public class Aulas
    {
        [Key]
        public int Id { get; set; }

        public required TimeOnly Duracao { get; set; }

        public required int CadeiraFK { get; set; }
        [ForeignKey("CadeiraFK")]
        public Cadeiras? Cadeira { get; set; }

        public required int TipologiaFK { get; set; }
        [ForeignKey("TipologiaFK")]
        public Tipologias? Tipologia { get; set; }

        public required int TurmaFK { get; set; }
        [ForeignKey("TurmaFK")]
        public Turmas? Turma { get; set; }

        public required string ProfessorFK { get; set; }
        [ForeignKey("ProfessorFK")]
        public IdentityUser? Professor { get; set; }
    }
}
