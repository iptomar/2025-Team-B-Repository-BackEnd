using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Horario
    {
        [Key]
        public int id_horario { get; set; }

        [ForeignKey(nameof(curso))]
        public int id_curso { get; set; }
        public Curso curso { get; set; }

        [Required]
        public DateOnly data_inicio { get; set; }

        [Required]
        public DateOnly data_fim { get; set; }

        public ICollection<Registo> FKRegisto { get; set; }

        public ICollection<Bloco_Horario> FKBlocoHorario { get; set;}
    }
}
