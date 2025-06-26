using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPT_Teste.Models
{
    public class Instituicoes
    {
        [Key]
        public int Id { get; set; }

        public required string Instituicao { get; set; }

        public int LocalidadeFK { get; set; }
        [ForeignKey("LocalidadeFK")]
        public Localidades? Localidade { get; set; }
    }
}
