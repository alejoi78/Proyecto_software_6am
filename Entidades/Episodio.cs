using Proyecto_software_6am.Entidades;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Episodio
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdEpisodio { get; set; }

    [Required(ErrorMessage = "El nombre del episodio es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "La temporada es obligatoria")]
    public string Temporada { get; set; }

    [Required(ErrorMessage = "El link es obligatorio")]
    [Url(ErrorMessage = "Debe ser una URL válida")]
    public string Link { get; set; }

    public int? DuracionMin { get; set; }

    [ForeignKey("Serie")]
    public int IdSerie { get; set; }

    [JsonIgnore]
    [NotMapped] // 👈 Añade este atributo para ignorar completamente la propiedad
    public virtual Serie? Serie { get; set; }  // Propiedad de navegación (nullable)
}