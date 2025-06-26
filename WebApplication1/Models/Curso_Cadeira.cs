using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(Id_curso), nameof(Id_cadeira))]
    public class Curso_Cadeira
    {
        [ForeignKey(nameof(Curso))]
        public int Id_curso { get; set; }
        public Curso Curso { get; set; }

        [ForeignKey(nameof(Cadeira))]
        public int Id_cadeira { get; set; }
        public Cadeira Cadeira { get; set; }
    }
}
