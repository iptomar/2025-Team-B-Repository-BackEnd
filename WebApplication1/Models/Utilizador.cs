using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models 
{
    public class Utilizador
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public bool Estado { get; set; } = true;

        [Required]
        [StringLength(125)]
        public string Nome { get; set; }

        public DateOnly Data_Edit {  get; set; }


        // String de Ligação de Credenciais ASPNetUser
        public String IDAspNetUser { get; set; }


        
        public ICollection<Registo> FKRegisto { get; set; }

    }
}
