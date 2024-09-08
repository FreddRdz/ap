using System.ComponentModel.DataAnnotations;

namespace ap.Models
{
    public class Administrador
    {
        public int IdAdmin { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Correo { get; set; }

        [Required]
        public string Contrasenia { get; set; }
    }
}