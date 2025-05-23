using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Professor
    {
        [Key]
        public int Id_professor { get; set; }

        [Required]
        [StringLength(125)]
        public string Nome { get; set; }

        public ICollection<Professor_Cadeira> FKProfessor_Cadeira { get; set; } = new List<Professor_Cadeira>();
    }
}
