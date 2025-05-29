using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IPT_Teste.Models
{
    public class Blocos
    {
        [Key]
        public int Id { get; set; }

        public required TimeOnly Hora_Inicio { get; set; }

        public required int SalaFK { get; set; }
        [ForeignKey("SalaFK")]
        public Salas? Sala { get; set; }

        public required int AulaFK { get; set; }
        [ForeignKey("AulaFK")]
        public Aulas? Aula { get; set; }

        public ICollection<Horarios> Horarios { get; set; } = new List<Horarios>();
    }
}
