using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(id_curso), nameof(id_cadeira))]
    public class Curso_Cadeira
    {
        [ForeignKey(nameof(curso))]
        public int id_curso { get; set; }
        public Curso curso { get; set; }

        [ForeignKey(nameof(cadeira))]
        public int id_cadeira { get; set; }
        public Cadeira cadeira { get; set; }
    }
}
