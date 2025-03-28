using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Registo
    {
        [Key]
        public int Id_registo { get; set; }

        [ForeignKey(nameof(Id_utilizador))]
        public int Utilizadot { get; set; }
        public Utilizador Id_utilizador { get; set; }

        [ForeignKey(nameof(Id_horario))]
        public int Horario { get; set; }
        public Horario Id_horario { get; set; }

        [Required]
        [StringLength(1024)]
        public string Motivo { get; set; }
    }
}
