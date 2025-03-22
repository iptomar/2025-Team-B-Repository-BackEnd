using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Curso
    {
        [Key]
        public int id_curso { get; set; }

        [StringLength(255)]
        public string nome { get; set; }

        public int ano { get; set; }

        [ForeignKey(nameof(grau))]
        public int id_grau { get; set; }
        public Grau grau { get; set; }

        public ICollection<Horario> FKHorario { get; set; }   
        public ICollection<Turma> FKTurma { get; set; }
        public ICollection<Curso_Cadeira> FKCurso_Cadeira { get; set; }
        public ICollection<Curso_Instituicao> FKCurso_Instituicao { get; set; }
    }
}
