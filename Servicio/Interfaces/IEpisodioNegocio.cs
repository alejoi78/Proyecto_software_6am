using Proyecto_software_6am.Entidades;

namespace Proyecto_software_6am.Servicio.Interfaces
{
    public interface IEpisodioNegocio
    {
        Task<Boolean> guardarEpisodios(Episodio episodio);
        Task<Boolean> eliminarEpisodios(int id);
        Task<Boolean> buscarPorid(int id);
    }
}
