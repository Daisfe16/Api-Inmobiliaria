using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Api_Inmobiliaria.Models
{
    public class Pago
    {   [Key]
        [Column("id_pago")]
        public int IdPago { get; set; }
        [Column("id_contrato")]
        public int IdContrato { get; set; }
       [Column("fecha")]
        public DateTime Fecha { get; set; }
        [Column("importe")]
        public decimal Importe { get; set; }
        [Column("nro_pago")]
        public int NroPago { get; set; }
        [Column("detalle")]
        public string? Detalle { get; set; } = "";
        [Column("estado")]
        public bool Estado { get; set; } = true;

      

    }
}
