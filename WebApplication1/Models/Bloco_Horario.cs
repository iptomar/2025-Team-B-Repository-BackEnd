using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(id_bloco), nameof(id_horario))]
    public class Bloco_Horario
    {
        [ForeignKey(nameof(bloco))]
        public int id_bloco { get; set; }
        public Bloco bloco { get; set; }

        [ForeignKey(nameof(horario))]
        public int id_horario { get; set; }
        public Horario horario { get; set; }
    }
}
