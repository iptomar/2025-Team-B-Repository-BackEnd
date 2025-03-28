using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(Id_professor), nameof(Id_cadeira))]
    public class Professor_Cadeira
    {
        [ForeignKey(nameof(Professor))]
        public int Id_professor { get; set; }
        public Professor Professor { get; set; }

        [ForeignKey(nameof(Cadeira))]
        public int Id_cadeira { get; set; }
        public Cadeira Cadeira { get; set; }

    }
}
