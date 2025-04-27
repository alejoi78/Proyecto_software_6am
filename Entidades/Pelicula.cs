
using System.ComponentModel.DataAnnotations;

namespace Proyecto_software_6am.Entidades
{
    public class Pelicula : Contenido
    {
        [Key]
        public int IdPelicula { get; set; }
        [Range(0, 24)]
        public double DuracionHoras { get; set; }
    }
}

