using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(Letra_turma), nameof(Ano_academico), nameof(Semestre))]
    public class Turma
    {
        [StringLength(1)]
        public string Letra_turma { get; set; }

        public int Ano_academico { get; set; }

        public int Semestre { get; set; }

        [ForeignKey(nameof(Id_curso))]
        public int Curso { get; set; }
        public Curso Id_curso { get; set; }
    }
}
