using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(Id_bloco), nameof(Id_horario))]
    public class Bloco_Horario
    {
        [ForeignKey(nameof(Bloco))]
        public int Id_bloco { get; set; }
        public Bloco Bloco { get; set; }

        [ForeignKey(nameof(Horario))]
        public int Id_horario { get; set; }
        public Horario Horario { get; set; }
    }
}
