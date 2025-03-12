using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante_Moises_Loor.Models
{
    public class Orden
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Cliente? Usuario { get; set; }

        [Required]
        public required DateTime Fecha { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public required string MetodoPago { get; set; }

        public List<Detalle>? Detalles { get; set; }
    }
}
