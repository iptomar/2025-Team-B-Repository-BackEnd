using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Localidade
    {
        [Key]
        public int Id_localidade { get; set; }
        
        [StringLength(50)]
        public string Nome_localidade { get; set; }

        public ICollection<Sala> FKSala { get; set; }

    }
}



