using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Cadeira
    {
        [Key]
        public int id_cadeira { get; set; }

        [StringLength(50)]
        public string nome_cadeira { get; set; }

        public int carga_total { get; set; }

        public ICollection<Tipologia_Cadeira> FKTipologia_Cadeira { get; set; }
        public ICollection<Professor_Cadeira> FKProfessor_Cadeira { get; set; }
        public ICollection<Curso_Cadeira> FKCurso_Cadeira { get; set; }
    }
}
