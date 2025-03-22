using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Registo
    {
        [Key]
        public int id_registo { get; set; }

        [ForeignKey(nameof(utilizador))]
        public int id_utilizadot { get; set; }
        public Utilizador utilizador { get; set; }

        [ForeignKey(nameof(horario))]
        public int id_horario { get; set; }
        public Horario horario { get; set; }

        [Required]
        [StringLength(1024)]
        public string motivo { get; set; }
    }
}
