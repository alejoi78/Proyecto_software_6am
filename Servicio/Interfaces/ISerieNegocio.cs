using Proyecto_software_6am.Entidades;

namespace Proyecto_software_6am.Servicio.Interfaces
{
    public interface ISerieNegocio
    {
        Task<List<Entidades.Serie>> listarSeries();
        Task<Boolean> guardarSeries(Serie serie);
        Task<Boolean> actualizarSeries(Serie serie);
        Task<Boolean> eliminarSeries(int id);
    }
}
