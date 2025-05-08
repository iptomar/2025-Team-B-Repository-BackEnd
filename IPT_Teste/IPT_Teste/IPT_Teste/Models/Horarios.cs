using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IPT_Teste.Models
{
    public class Horarios
    {
        [Key]
        public int Id { get; set; }

        public required DateOnly Inicio { get; set; }

        public required DateOnly Fim { get; set; }

        public ICollection<Blocos> Blocos { get; set; } = new List<Blocos>();
    }
}
