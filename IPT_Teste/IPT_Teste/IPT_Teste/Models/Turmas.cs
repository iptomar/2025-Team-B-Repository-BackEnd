using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPT_Teste.Models
{
    public class Turmas
    {
        [Key]
        public int Id { get; set; }

        public required string Letra { get; set; }

        public required int Ano { get; set; }

        public required int Semestre { get; set; }

        public required int CursoFK { get; set; }
        [ForeignKey("CursoFK")]
        public Cursos? Curso { get; set; }
    }
}
