using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Servicio;
using Proyecto_software_6am.Servicio.Interfaces;
using System.Text.Json;
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

    [HttpGet]
    [Route("obtenerPorId")]
    public async Task<IActionResult> obtenerPorId(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            Console.WriteLine($"Buscando película con ID: {id}");

            var pelicula = await _pelicula.obtenerPorId(id);

            if (pelicula == null)
                return NotFound($"No se encontró la película con ID: {id}");

            // Debug: Mostrar datos obtenidos
            Console.WriteLine($"Datos obtenidos: {JsonSerializer.Serialize(pelicula)}");

            return Ok(pelicula);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al obtener película: {ex.Message}");
            return StatusCode(500, "Error interno del servidor al obtener la película");
        }
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

    [HttpDelete]
    [Route("eliminar")]
    public async Task<IActionResult> eliminarPelicula(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            Console.WriteLine($"Intentando eliminar película con ID: {id}");

            bool eliminado = await _pelicula.eliminarPeliculas(id);

            if (!eliminado)
                return NotFound("No se encontró la película o no se pudo eliminar");

            return Ok(new
            {
                success = true,
                message = "Película eliminada correctamente"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al eliminar película: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

}

