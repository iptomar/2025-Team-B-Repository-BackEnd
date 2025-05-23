using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(Id_curso), nameof(Id_instituicao))]
    public class Curso_Instituicao
    {
        public Curso_Instituicao(int id_curso, int id_instituicao)
        {
            Id_curso = id_curso;
            Id_instituicao = id_instituicao;
        }

        [ForeignKey(nameof(Curso))]
        public int Id_curso { get; set; }
        public Curso Curso { get; set; }

        [ForeignKey(nameof(Instituicao))]
        public int Id_instituicao { get; set; }
        public Instituicao Instituicao { get; set; }
    }
}
