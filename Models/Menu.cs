using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante_Moises_Loor.Models
{
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Nombre { get; set; }

        public required string Descripcion { get; set; }

        [Required]
        [Precision(18, 2)]
        public required decimal Precio { get; set; }

        [Required]
        public required bool Estado { get; set; }
    }
}
