using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Inmobiliaria.Models{
    public class Tipo
    {   [Key]
        [Column("id_tipo")]
        public int IdTipo { get; set; }
        [Column("nombre")]
        public string? Nombre { get; set; } ="";
    }
}
