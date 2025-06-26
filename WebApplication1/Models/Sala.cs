using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Sala
    {
        [Key]
        public int Id_sala { get; set; }
        
        public int localidade { get; set; }
        [ForeignKey(nameof(localidade))]
        public Localidade Id_localidade { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Nome_sala { get; set; }

        public ICollection<Bloco> FKBloco { get; set; }

    }
}
