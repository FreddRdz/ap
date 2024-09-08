using System.ComponentModel.DataAnnotations;

namespace ap.Models
{
    public class Vehiculo
    {
        public int IdVehiculo { get; set; }

        [Required]
        public int IdCliente { get; set; }  // Relacionado con Cliente

        [Required]
        [StringLength(50)]
        public string Marca { get; set; }

        [Required]
        [StringLength(50)]
        public string Modelo { get; set; }

        [Required]
        public int Anio { get; set; }

        [Required]
        [StringLength(20)]
        public string Placa { get; set; }
    }
}