using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Professor
    {
        [Key]
        public int id_professor { get; set; }

        [StringLength(20)]
        public string nome { get; set; }

        [StringLength(20)]
        public string apelido { get; set; }

        public ICollection<Professor_Cadeira> FKProfessor_Cadeira { get; set; }
    }
}
