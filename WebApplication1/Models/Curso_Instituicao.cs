using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(Id_curso), nameof(Id_instituicao))]
    public class Curso_Instituicao
    {
        [ForeignKey(nameof(Curso))]
        public int Id_curso { get; set; }
        public Curso Curso { get; set; }

        [ForeignKey(nameof(Instituicao))]
        public int Id_instituicao { get; set; }
        public Instituicao Instituicao { get; set; }
    }
}
