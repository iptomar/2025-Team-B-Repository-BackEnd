using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Grau
    {
        [Key]
        public int id_grau { get; set; }

        [StringLength(50)]
        public string nome_grau { get; set; }

        public ICollection<Curso> curso { get; set; }
    }
}
