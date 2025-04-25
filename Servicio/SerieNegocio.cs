using Proyecto_software_6am.DAOs.Interfaces;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Servicio.Interfaces;

namespace Proyecto_software_6am.Servicio
{
    public class SerieNegocio: ISerieNegocio
    {
        private readonly ISerieDAO _serieDAO;

        public SerieNegocio(ISerieDAO seriesDAO)
        {
            _serieDAO = seriesDAO;
        }

        public async Task<List<Serie>> listarSeries()
        {
            List<Serie> result = await _serieDAO.listarSeries();
            return result;
        }

        public async Task<bool> guardarSeries(Serie serie)
        {
            return await _serieDAO.guardarSeries(serie);
        }

        public async Task<bool> actualizarSeries(Serie serie)
        {
            return await _serieDAO.actualizarSeries(serie);
        }
    
    }
}
