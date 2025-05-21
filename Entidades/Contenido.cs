using System.ComponentModel.DataAnnotations;

namespace Proyecto_software_6am.Entidades
{
    public abstract class Contenido
    {


        //[Required]
        // [MaxLength(200)]
        public string Titulo { get; set; }

        // [Required]
        //  [MaxLength(100)]
        public string Director { get; set; }
        //
        [Range(1900, 2100)]
        public int Anio { get; set; }

        // [Url]
        public string Link { get; set; }
        public string Genero { get; set; } = string.Empty;
        public double Calificacion { get; set; }

        public string Imagen { get; set; }

        public Contenido() { }
    }
}