using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante_Moises_Loor.Models
{
    public class Detalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required int OrdenId { get; set; }

        [ForeignKey("OrdenId")]
        public Orden? Orden { get; set; }

        [Required]
        public required int MenuId { get; set; }

        [ForeignKey("MenuId")]
        public Menu? Menu { get; set; }

        [Required]
        public required int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }
    }
}
