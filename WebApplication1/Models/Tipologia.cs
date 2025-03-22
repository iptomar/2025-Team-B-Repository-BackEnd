using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{

    public class Tipologia
    {
        [Key]
        public int id_tipologia { get; set; }

        [StringLength(20)]
        public string tipologia { get; set; }

        public ICollection<Tipologia_Cadeira> FKTipologia_Cadeira {  get; set; }
    }
}
