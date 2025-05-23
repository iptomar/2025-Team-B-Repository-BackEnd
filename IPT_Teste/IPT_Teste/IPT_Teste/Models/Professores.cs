using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPT_Teste.Models
{
    public class Professores
    {
        [Key]
        public int Id { get; set; }

        public int CursoFK { get; set; }
        [ForeignKey("CursoFK")]
        public Cursos? Curso { get; set; }

        [Required]
        public required string ProfessorFK { get; set; }
        [ForeignKey("ProfessorFK")]
        public IdentityUser? Professor { get; set; }
    }
}
