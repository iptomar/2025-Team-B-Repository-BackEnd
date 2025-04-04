using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models 
{
    [PrimaryKey(nameof(Id_utilizador))]
    public class Utilizador
    {
        [Required]
        public bool Estado { get; set; }

        [Required]
        [StringLength(125)]
        public string Nome { get; set; }

        // Adicionar Ligação com AspNetUsers NÂO SEI COMO FAZER
        //[ForeignKey(nameof(Utilizador))]
        public int Id_utilizador { get; set; }
        //public ApplicationUser Utilizador { get; set; }

        public ICollection<Registo> FKRegisto { get; set; }

    }
}
