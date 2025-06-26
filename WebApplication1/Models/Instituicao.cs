using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Instituicao
    {
        [Key]
        public int Id_instituicao { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome_instituicao { get; set; }

        public ICollection<Curso_Instituicao> FKCurso_Instituicao { get; set; }
    }
}
