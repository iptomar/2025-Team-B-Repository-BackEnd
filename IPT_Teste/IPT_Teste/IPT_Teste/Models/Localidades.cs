using System.ComponentModel.DataAnnotations;

namespace IPT_Teste.Models
{
    public class Localidades
    {
        [Key]
        public int Id { get; set; }

        public required string Localidade { get; set; }
    }
}
