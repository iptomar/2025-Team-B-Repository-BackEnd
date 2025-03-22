using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Instituicao
    {
        [Key]
        public int id_instituicao { get; set; }

        [StringLength(100)]
        public string nome_instituicao { get; set; }

        public ICollection<Curso_Instituicao> FKCurso_Instituicao { get; set; }
    }
}
