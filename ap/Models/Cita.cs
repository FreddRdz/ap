using System;
using System.ComponentModel.DataAnnotations;

namespace ap.Models
{
    public class Cita
    {
        public int IdCita { get; set; }

        [Required]
        public int IdVehiculo { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public DateTime? FechaTerminacion { get; set; }

        [Required]
        public bool? Estado { get; set; }

        public string Comentarios { get; set; }

        public string Vehiculo { get; set; }
    }
}