using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante_Moises_Loor.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El campo apellido es requerido")]
        public required string Apellido { get; set; }

        public string? Correo { get; set; }

        public string? Telefono { get; set; }

        public List<Orden>? Ordenes { get; set; }

    }
}
