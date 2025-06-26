using System.ComponentModel.DataAnnotations;

namespace IPT_Teste.Models
{
    public class Tipologias
    {
        [Key]
        public int Id { get; set; }

        public required string Tipologia { get; set; }
    }
}
