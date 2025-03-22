using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(id_tipologia), nameof(id_cadeira))]
    public class Tipologia_Cadeira
    {

        [ForeignKey(nameof(tipologia))]
        public int id_tipologia { get; set; }
        public Tipologia tipologia { get; set; }

        [ForeignKey(nameof(cadeira))]
        public int id_cadeira { get; set; }
        public Cadeira cadeira { get; set; }


    }
}
