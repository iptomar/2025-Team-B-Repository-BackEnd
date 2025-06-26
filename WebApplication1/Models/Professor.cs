using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Professor
    {
        [Key]
        public int Id_professor { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }

        [Required]
        [StringLength(20)]
        public string Apelido { get; set; }

        public ICollection<Professor_Cadeira> FKProfessor_Cadeira { get; set; }
    }
}
