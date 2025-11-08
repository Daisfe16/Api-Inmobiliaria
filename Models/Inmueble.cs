using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Api_Inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api_Inmobiliaria.Models
{
	[Table("Inmueble")]
	public class Inmueble
	{
        [Key]
        [Column("id_inmueble")]
		public int IdInmueble { get; set; }
		//[Required]
		
		[Required(ErrorMessage = "La dirección es requerida")]
		public string? Direccion { get; set; }
		[Column("ambiente")]
		[Required]
		public int Ambientes { get; set; }
		
		[Column("latitud")]
		public int Latitud { get; set; }
		[Column ("longitud")]
		public int Longitud { get; set; }
		[Column("precio")]
		public int Precio { get; set; }
		[Column("id_tipo")]
		public int IdTipo { get; set; }

		[ForeignKey(nameof(IdTipo))]
		public Tipo? Tipo { get; set; }

		[Column("id_propietario")]
		public int PropietarioId { get; set; }
		[ForeignKey(nameof(PropietarioId))]
		[BindNever]
		public Propietario? Duenio { get; set; }

		[Column("imagen")]

		public string? Imagen { get; set; } = "";
		[Column("habilitado")]
		public bool Habilitado { get; set; } = true;
	}
	
}