using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ap.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string CorreoElectronico { get; set; }

        [Required]
        public int? Telefono { get; set; }

        [Required]
        [StringLength(255)]
        public string Direccion { get; set; }


        public bool IsNew { get; set; }

        public List<Vehiculo> Vehiculos { get; set; }
    }
}