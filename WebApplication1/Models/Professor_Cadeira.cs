using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(id_professor), nameof(id_cadeira))]
    public class Professor_Cadeira
    {
        [ForeignKey(nameof(professor))]
        public int id_professor { get; set; }
        public Professor professor { get; set; }

        [ForeignKey(nameof(cadeira))]
        public int id_cadeira { get; set; }
        public Cadeira cadeira { get; set; }

    }
}
