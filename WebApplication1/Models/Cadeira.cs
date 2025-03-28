using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Cadeira
    {
        [Key]
        public int Id_cadeira { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome_cadeira { get; set; }

        [Required]
        public int Carga_total { get; set; }

        public ICollection<Tipologia_Cadeira> FKTipologia_Cadeira { get; set; }
        public ICollection<Professor_Cadeira> FKProfessor_Cadeira { get; set; }
        public ICollection<Curso_Cadeira> FKCurso_Cadeira { get; set; }
    }
}
