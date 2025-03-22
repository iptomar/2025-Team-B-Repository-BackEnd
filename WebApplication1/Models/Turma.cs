using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(letra_turma), nameof(ano_academico), nameof(semestre))]
    public class Turma
    {
        public string letra_turma { get; set; }

        public int ano_academico { get; set; }

        public int semestre { get; set; }

        [ForeignKey(nameof(curso))]
        public int id_curso { get; set; }
        public Curso curso { get; set; }
    }
}
