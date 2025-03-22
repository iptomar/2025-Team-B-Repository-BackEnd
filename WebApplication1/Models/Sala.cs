using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Sala
    {
        [Key]
        public int id_sala { get; set; }

        [ForeignKey(nameof(localidade))]
        public int id_localidade { get; set; }
        public Localidade localidade { get; set; }

        [StringLength(20)]
        public string nome_sala { get; set; }

        public ICollection<Bloco> FKBloco { get; set; }

    }
}
