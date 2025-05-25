using Microsoft.AspNetCore.Mvc;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Servicio.Interfaces;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proyecto_software_6am.Controllers;

[ApiController]
[Route("episodios")]
public class EpisodioController : ControllerBase
{
    private readonly IEpisodioNegocio _episodioNegocio;

    public EpisodioController(IEpisodioNegocio episodioNegocio)
    {
        _episodioNegocio = episodioNegocio;
    }

    [HttpPost]
    [Route("guardar")]
    public async Task<IActionResult> GuardarEpisodio([FromBody] Episodio episodio)
    {
        try
        {
            // Validaciones básicas
            if (episodio == null)
                return BadRequest("Datos de episodio inválidos");

            Console.WriteLine($"Datos a guardar: {JsonSerializer.Serialize(episodio)}");

            bool resultado = await _episodioNegocio.guardarEpisodios(episodio);

            if (!resultado)
                return StatusCode(500, "Error al registrar el episodio");

            return Ok(new
            {
                success = true,
                message = "Episodio registrado correctamente",
                data = new { episodio.Nombre, episodio.Temporada }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en registro de episodio: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete]
    [Route("eliminar/{id}")]
    public async Task<IActionResult> EliminarEpisodio(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            Console.WriteLine($"Intentando eliminar episodio con ID: {id}");

            bool eliminado = await _episodioNegocio.eliminarEpisodios(id);

            if (!eliminado)
                return NotFound("No se encontró el episodio o no se pudo eliminar");

            return Ok(new
            {
                success = true,
                message = "Episodio eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar episodio: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet]
    [Route("existe/{id}")]
    public async Task<IActionResult> BuscarEpisodioPorId(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            Console.WriteLine($"Buscando episodio con ID: {id}");

            bool existe = await _episodioNegocio.buscarPorid(id);

            return Ok(new
            {
                success = true,
                existe = existe
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al buscar episodio: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}