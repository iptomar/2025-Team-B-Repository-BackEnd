using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IPT_Teste.Models
{
    public enum EstadoHorario
    {
        EDITAVEL = 0,
        POR_APROVAR = 1,
        APROVADO = 2
    }

    public class Horarios
    {
        [Key]
        public int Id { get; set; }

        public required DateOnly Inicio { get; set; }

        public required DateOnly Fim { get; set; }

        public int? TurmaFK { get; set; }
        [ForeignKey("TurmaFK")]
        public Turmas? Turma { get; set; }

        public EstadoHorario? Estado { get; set; }

        public ICollection<Blocos> Blocos { get; set; } = new List<Blocos>();
    }
}
