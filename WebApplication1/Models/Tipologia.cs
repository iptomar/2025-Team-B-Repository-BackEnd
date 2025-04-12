using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{

    public class Tipologia
    {
        [Key]
        public int Id_tipologia { get; set; }

        [StringLength(20)]
        public string Nome_tipologia { get; set; }

        public ICollection<Aula> FKAula {  get; set; }
    }
}
