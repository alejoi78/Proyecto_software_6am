using Proyecto_software_6am.DAOs;
using Proyecto_software_6am.DAOs.Interfaces;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Servicio.Interfaces;
using System;
using System.Threading.Tasks;

namespace Proyecto_software_6am.Servicio
{
    public class EpisodioNegocio : IEpisodioNegocio
    {
        private readonly IEpisodioDAO _episodioDAO;

        public EpisodioNegocio(IEpisodioDAO episodioDAO)
        {
            _episodioDAO = episodioDAO;
        }

        public async Task<bool> guardarEpisodios(Episodio episodio)
        {
            // Validaciones de negocio
            if (episodio == null)
                throw new ArgumentNullException(nameof(episodio), "El episodio no puede ser nulo");

            if (string.IsNullOrWhiteSpace(episodio.Nombre))
                throw new ArgumentException("El nombre del episodio es requerido", nameof(episodio.Nombre));

            if (string.IsNullOrWhiteSpace(episodio.Temporada))
                throw new ArgumentException("La temporada es requerida", nameof(episodio.Temporada));

            if (string.IsNullOrWhiteSpace(episodio.Link))
                throw new ArgumentException("El link es requerido", nameof(episodio.Link));

            return await _episodioDAO.guardarEpisodios(episodio);
        }

        public async Task<bool> eliminarEpisodios(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que cero", nameof(id));

            return await _episodioDAO.eliminarEpisodios(id);
        }

        public async Task<bool> buscarPorid(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que cero", nameof(id));

            return await _episodioDAO.buscarPorid(id);
        }
    }
}