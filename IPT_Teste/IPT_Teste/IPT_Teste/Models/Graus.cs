using System.ComponentModel.DataAnnotations;

namespace IPT_Teste.Models
{
    public class Graus
    {
        [Key]
        public int Id { get; set; }

        public required string Grau { get; set; }
    }
}
