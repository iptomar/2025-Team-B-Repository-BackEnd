using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Bloco
    {
        [Key]
        public int id_bloco { get; set; }

        [Required]
        [StringLength(5)]
        public string hora_inicio { get; set; }

        [Required]
        [StringLength(5)]
        public string hora_fim { get; set; }

        [ForeignKey(nameof(sala))]
        public int id_sala { get; set; }
        public Sala sala { get; set; }

        public ICollection<Bloco_Horario> FKBlocoHorario { get; set; }
    }
}
