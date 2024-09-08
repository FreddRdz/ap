using System;
using System.ComponentModel.DataAnnotations;

namespace ap.Models
{
    public class CitasAdmin
    {
        public int IdCitaAdmin { get; set; }

        [Required]
        public int IdCita { get; set; }  // Relacionado con Cita

        [Required]
        public int IdAdmin { get; set; }  // Relacionado con Administrador

        public string ComentariosAdmin { get; set; }

        [Required]
        public DateTime FechaAprobacion { get; set; }

        [Required]
        public bool Estado { get; set; }  // 1 para Aprobado, 0 para Rechazado
    }
}