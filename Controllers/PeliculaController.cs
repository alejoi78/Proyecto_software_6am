using Microsoft.AspNetCore.Mvc;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Servicio;
using Proyecto_software_6am.Servicio.Interfaces;
namespace Proyecto_software_6am.Controllers;

    [ApiController]
    [Route("peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaNegocio _pelicula;

        public PeliculasController(IPeliculaNegocio pelicula)
        {
        _pelicula = pelicula;
        }

    [HttpGet]
    [Route("listar")]
    public Task<List<Pelicula>> Get()
    {
        return _pelicula.listarPeliculas();
    }

    [HttpPost]
    [Route("nuevo")]
    public IActionResult Post(Pelicula pelicula)
    {
        Task<bool> result = _pelicula.guardarPeliculas(pelicula);
        if (result.IsCompleted)
        {
        }
        Console.WriteLine(" insertado ");
        return Ok();

    }

    [HttpPut]
    [Route("actualizar")]
    public IActionResult actualizarPeliculas(Pelicula pelicula)
    {
        Task<bool> result = _pelicula.actualizarPeliculas(pelicula);
        Console.WriteLine(" actualizado ");
        return Ok();
    }
}

