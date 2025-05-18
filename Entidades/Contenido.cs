using System.ComponentModel.DataAnnotations;

namespace Proyecto_software_6am.Entidades
{
    public abstract class Contenido
    {
        
      
        public string Titulo { get; set; }
        
        public string Director { get; set; }
        
        [Range(1900, 2100)]
        public int Anio { get; set; }

        public string Link { get; set; }
        public string Genero { get; set; }


        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5")]
        public double Calificacion { get; set; }

        public Contenido() { }
    }
}

