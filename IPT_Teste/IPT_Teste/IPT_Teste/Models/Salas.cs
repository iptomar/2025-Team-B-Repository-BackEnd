using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPT_Teste.Models
{
    public class Salas
    {
        [Key]
        public int Id { get; set; }

        public required string Sala { get; set; }

        public int LocalidadeFK { get; set; }
        [ForeignKey("LocalidadeFK")]
        public Localidades? Localidade { get; set; }
    }
}
