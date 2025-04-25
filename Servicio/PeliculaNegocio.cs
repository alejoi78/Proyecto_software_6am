using Proyecto_software_6am.DAOs.Interfaces;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Servicio.Interfaces;

public class PeliculaServicio : IPeliculaNegocio
{
    private readonly IPeliculaDAO _peliculasDAO;

    public PeliculaServicio(IPeliculaDAO peliculasDAO)
    {
        _peliculasDAO = peliculasDAO;
    }

    public async Task<List<Pelicula>> listarPeliculas()
    {
        List<Pelicula> result = await _peliculasDAO.listarPeliculas();
        return result;
    }

    public async Task<bool> guardarPeliculas(Pelicula pelicula)
    {
        return await _peliculasDAO.guardarPeliculas(pelicula);
    }

    public async Task<bool> actualizarPeliculas(Pelicula pelicula)
    {
        return await _peliculasDAO.actualizarPeliculas(pelicula);
    }
}