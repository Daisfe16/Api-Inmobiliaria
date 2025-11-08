using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Api_Inmobiliaria.Models
{
	
	public class Propietario
	{
		[Key]
        
        [Column("id_propietario")]
        public int IdPropietario { get; set; }
		[Required]
		[Column("nombre")]
		public string Nombre { get; set; } = "";
		[Required]
		[Column("apellido")]
		public string Apellido { get; set; } = "";
		[Required]
		[Column("dni")]
		public string Dni { get; set; } = "";
		[Column("telefono")]
		public string Telefono { get; set; } = "";
		[Required, EmailAddress]
		[Column("Email")]
		public string Email { get; set; } = "";
		[Required(ErrorMessage = "La clave es obligatoria"), DataType(DataType.Password)]
		[Column("clave")]
		public string Clave { get; set; } = "";


		public override string ToString()
		{
			var res = $"{Nombre} {Apellido}";
			if(!String.IsNullOrEmpty(Dni)) {
				res += $" ({Dni})";
			}
			return res;
		}
	}
}