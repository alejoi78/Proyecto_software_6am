using Proyecto_software_6am.Entidades;

namespace Proyecto_software_6am.DAOs.Interfaces
{
    public interface IPeliculaDAO
    {
        Task<List<Entidades.Pelicula>> listarPeliculas();
        Task<Boolean> guardarPeliculas(Pelicula pelicula);
        Task<Boolean> actualizarPeliculas(Pelicula pelicula);
        Task<Boolean> eliminarPeliculas(int id);
        Task<Pelicula> obtenerPorId(int id);
    }
}
