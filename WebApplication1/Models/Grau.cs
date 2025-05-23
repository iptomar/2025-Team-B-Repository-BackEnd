using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Grau
    {
        [Key]
        public int Id_grau { get; set; }

        [StringLength(50)]
        public string Nome_grau { get; set; }

        public ICollection<Curso> Curso { get; set; } = new List<Curso>();
    }
}
