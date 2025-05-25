using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_software_6am.Entidades
{
    public class Serie : Contenido
    {
        [Key]
        public int IdSerie { get; set; }

        [Range(1, 30, ErrorMessage = "Las temporadas deben ser entre 1 y 30.")]
        public int Temporadas { get; set; }

        public double DuracionPorCapitulo { get; set; }

        public virtual ICollection<Episodio> Episodios { get; set; } // Relación uno a muchos
    }
}