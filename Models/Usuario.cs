using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaMVC.Models
{
    public class Usuario
    {   
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }
        [Column("email")]
        public string Email { get; set; } = "";
        [Column("password_hash")]
        public string PasswordHash { get; set; } = "";
        [Column("nombre")]
        public string Nombre { get; set; } = "";
        [Column("apellido")]
        public string Apellido { get; set; } = "";
        [Column("avatar_path")]
        public string? AvatarPath { get; set; } = "/images/imgdef.png";
        [Column("rol")]
        public string Rol { get; set; } = ""; 
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
