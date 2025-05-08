using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IPT_Teste.Models
{
    public class Cadeiras
    {
        [Key]
        public int Id { get; set; }

        public required string Cadeira { get; set; }

        public required int Ano { get; set; }

        public required int Semestre { get; set; }

        public required int ECTS { get; set; }

        [JsonIgnore]
        public ICollection<Cursos> Cursos { get; set; } = new List<Cursos>();
    }
}
