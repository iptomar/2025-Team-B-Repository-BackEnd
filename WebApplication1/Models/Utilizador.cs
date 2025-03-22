using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Utilizador
    {
        [Key]
        public int id_utilizador { get; set; }

        [Required]
        public int role { get; set; }

        [Required]
        [StringLength(255)]
        public string email { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [Required]
        public bool estado { get; set; }

        public ICollection<Registo> FKRegisto { get; set; }
    }
}
