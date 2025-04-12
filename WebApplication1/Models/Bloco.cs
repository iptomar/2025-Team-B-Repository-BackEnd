using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Bloco
    {
        [Key]
        public int Id_bloco { get; set; }

        [Required]
        public TimeOnly Hora_inicio { get; set; }

        [ForeignKey(nameof(Id_sala))]
        public int Sala { get; set; }
        public Sala Id_sala { get; set; }

        public ICollection<Bloco_Horario> FKBlocoHorario { get; set; }
    }
}
