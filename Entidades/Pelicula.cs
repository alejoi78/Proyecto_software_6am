
using System.ComponentModel.DataAnnotations;

namespace Proyecto_software_6am.Entidades
{
    public class Pelicula : Contenido
    {
        [Range(0, 24)]
        public double DuracionHoras { get; set; }
    }
}

