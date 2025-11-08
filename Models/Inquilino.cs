
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Api_Inmobiliaria.Models
{
	public class Inquilino
	{
		[Key]
        [Display(Name = "Código")]
        
        [Column("id_inquilino")]
		public int IdInquilino { get; set; }
		[Required]
		[Column("nombre")]
		public string Nombre { get; set; }
		[Required]
		[Column("apellido")]
		public string Apellido { get; set; }
		[Required]
		[Column("dni")]
		public string Dni { get; set; }
		[Column("telefono")]
		public string Telefono { get; set; }
		[Required, EmailAddress]
		[Column("email")]
		public string Email { get; set; }
	}
}