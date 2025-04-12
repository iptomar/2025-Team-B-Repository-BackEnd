using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(Id_tipologia), nameof(Id_cadeira))]
    public class Aula
    {

        [ForeignKey(nameof(Tipologia))]
        public int Id_tipologia { get; set; }
        public Tipologia Tipologia { get; set; }

        [ForeignKey(nameof(Cadeira))]
        public int Id_cadeira { get; set; }
        public Cadeira Cadeira { get; set; }

        [ForeignKey(nameof(Professor))]
        public int Id_professor { get; set; }
        public Professor Professor { get; set; }

        public int Duracao {  get; set; }

    }
}
