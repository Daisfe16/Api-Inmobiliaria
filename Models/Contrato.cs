using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Api_Inmobiliaria.Models
{
    public class Contrato
    {
        [Key]
        [Column("id_contrato")]
        public int IdContrato { get; set; }

        [Required]
        [Column("id_inquilino")]
        public int IdInquilino { get; set; }

        [Required]
        [Column("id_inmueble")]
        public int IdInmueble { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column("fecha_desde")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column("fecha_hasta")]
        public DateTime FechaFin { get; set; }
        [Column("estado")]
        public bool Estado { get; set; } = true;

        [Required]
        [Column("monto")]
        public int Monto { get; set; }

    
        [ForeignKey(nameof(IdInmueble))]
        public Inmueble? Inmueble { get; set; }
        [ForeignKey(nameof(IdInquilino))]
        public Inquilino? Inquilino { get; set; }
        
    }

}
