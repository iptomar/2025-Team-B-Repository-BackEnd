using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(id_curso), nameof(id_instituicao))]
    public class Curso_Instituicao
    {
        [ForeignKey(nameof(curso))]
        public int id_curso { get; set; }
        public Curso curso { get; set; }

        [ForeignKey(nameof(instituicao))]
        public int id_instituicao { get; set; }
        public Instituicao instituicao { get; set; }
    }
}
