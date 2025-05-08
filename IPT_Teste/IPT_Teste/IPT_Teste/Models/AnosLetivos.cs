using System.ComponentModel.DataAnnotations;

namespace IPT_Teste.Models
{
    public class AnosLetivos
    {
        [Key]
        public int Id { get; set; }

        public required string AnoLetivo { get; set; }
    }
}
