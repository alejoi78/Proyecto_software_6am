using Microsoft.AspNetCore.Mvc;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Servicio;
using Proyecto_software_6am.Servicio.Interfaces;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proyecto_software_6am.Controllers;

[ApiController]
[Route("series")]
public class SerieController : ControllerBase
{
    private readonly ISerieNegocio _serie;

    public SerieController(ISerieNegocio serie)
    {
        _serie = serie;
    }

    [HttpGet]
    [Route("listar")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var series = await _serie.listarSeries();

            // Debug: Mostrar datos obtenidos
            Console.WriteLine($"Datos obtenidos: {JsonSerializer.Serialize(series)}");

            if (series == null || !series.Any())
                return NotFound("No se encontraron series");

            return Ok(series);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GET /series/listar: {ex.Message}");
            return StatusCode(500, "Error al obtener las series");
        }
    }

    [HttpGet]
    [Route("obtenerPorId")]
    public async Task<IActionResult> obtenerPorId(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            Console.WriteLine($"Buscando serie con ID: {id}");

            var serie = await _serie.obtenerPorId(id);

            if (serie == null)
                return NotFound($"No se encontró la serie con ID: {id}");

            // Debug: Mostrar datos obtenidos
            Console.WriteLine($"Datos obtenidos: {JsonSerializer.Serialize(serie)}");

            return Ok(serie);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al obtener serie: {ex.Message}");
            return StatusCode(500, "Error interno del servidor al obtener la serie");
        }
    }

    [HttpPost]
    [Route("nuevo")]
    public async Task<IActionResult> Create([FromBody] Serie serie)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validación manual adicional
            if (serie.Episodios != null)
            {
                foreach (var episodio in serie.Episodios)
                {
                    episodio.IdSerie = 0; // Resetear el ID temporal
                    episodio.Serie = null; // Asegurar que la propiedad Serie sea nula
                }
            }

            bool resultado = await _serie.guardarSeries(serie);

            if (!resultado)
            {
                return StatusCode(500, "Error al guardar la serie");
            }

            return Ok(new
            {
                success = true,
                message = "Serie creada exitosamente",
                data = serie
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    [HttpPut]
    [Route("actualizar")]
    public async Task<IActionResult> actualizarSeries([FromBody] Serie serie)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (serie.IdSerie <= 0)
            {
                return BadRequest("ID de serie inválido");
            }

            Console.WriteLine($"Actualizando serie ID: {serie.IdSerie}");

            // Validación y preparación de episodios
            if (serie.Episodios != null)
            {
                foreach (var episodio in serie.Episodios)
                {
                    episodio.IdSerie = serie.IdSerie; // Asignar el ID correcto
                    episodio.Serie = null; // Evitar problemas de referencia circular
                }
            }

            bool resultado = await _serie.actualizarSeries(serie);

            if (!resultado)
            {
                return StatusCode(500, "Error al actualizar la serie");
            }

            // Obtener la serie actualizada para devolverla
            var serieActualizada = await _serie.obtenerPorId(serie.IdSerie);

            return Ok(new
            {
                success = true,
                message = "Serie actualizada exitosamente",
                data = serieActualizada
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al actualizar serie: {ex.Message}");
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    [HttpDelete]
    [Route("eliminar")]
    public async Task<IActionResult> eliminarSeries(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            Console.WriteLine($"Intentando eliminar serie con ID: {id}");

            bool eliminado = await _serie.eliminarSeries(id);

            if (!eliminado)
                return NotFound("No se encontró la serie o no se pudo eliminar");

            return Ok(new
            {
                success = true,
                message = "Serie eliminada correctamente"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al eliminar serie: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}