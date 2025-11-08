using System.ComponentModel.DataAnnotations;

namespace Api_Inmobiliaria.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Clave{ get; set;  }



    }
}