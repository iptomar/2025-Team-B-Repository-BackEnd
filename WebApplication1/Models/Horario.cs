using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Horario
    {
        [Key]
        public int Id_horario { get; set; }

        [ForeignKey(nameof(Id_curso))]
        public int Curso { get; set; }
        public Curso Id_curso { get; set; }

        [Required]
        public DateOnly Data_inicio { get; set; }

        [Required]
        public DateOnly Data_fim { get; set; }

        public ICollection<Registo> FKRegisto { get; set; } = new List<Registo>();

        public ICollection<Bloco_Horario> FKBlocoHorario { get; set;} = new List<Bloco_Horario>();
    }
}
