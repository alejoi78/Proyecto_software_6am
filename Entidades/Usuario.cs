using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_software_6am.Entidades;
public class Usuario
{
    [Key]  // Clave primaria
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    [MaxLength(100)]
    public string Correo { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    [DataType(DataType.Password)]  // Para enmascarar en vistas (no afecta la BD)
    public string Contraseña { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]  
    public string Rol { get; set; } = "Usuario";  

    // Constructor vacío (requerido por EF Core)
    public Usuario() { }
}