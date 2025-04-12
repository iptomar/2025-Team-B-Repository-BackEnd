using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Curso
    {
        [Key]
        public int Id_curso { get; set; }

        [StringLength(255)]
        public string Nome { get; set; }

        [StringLength(7)]
        public string Ano { get; set; }

        [StringLength(450)]
        public string Coordenador { get; set; }

        [ForeignKey(nameof(Id_grau))]
        public int Grau { get; set; }
        public Grau Id_grau { get; set; } 

        public ICollection<Horario> FKHorario { get; set; }   
        public ICollection<Turma> FKTurma { get; set; }
        public ICollection<Curso_Cadeira> FKCurso_Cadeira { get; set; }
        public ICollection<Curso_Instituicao> FKCurso_Instituicao { get; set; }
    }
}
