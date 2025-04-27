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

    [HttpPost]
    [Route("nuevo")]
    public async Task<IActionResult> Post([FromBody] Serie serie)
    {
        try
        {
            // Validación básica
            if (serie == null)
                return BadRequest("Datos de serie inválidos");

            if (string.IsNullOrEmpty(serie.Titulo))
                return BadRequest("El título es requerido");

            if (serie.DuracionPorCapitulo <= 0)
                return BadRequest("La duración debe ser mayor que cero");

            // Debug: Mostrar datos recibidos
            Console.WriteLine($"Datos recibidos: {JsonSerializer.Serialize(serie)}");

            bool resultado = await _serie.guardarSeries(serie);

            if (!resultado)
            {
                Console.WriteLine("Error: No se pudo guardar en la base de datos");
                return StatusCode(500, "Error al guardar la serie");
            }

            return Ok(new
            {
                success = true,
                message = "Serie guardada correctamente",
                data = serie
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en POST /series/nuevo: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut]
    [Route("actualizar")]
    public IActionResult actualizarSeries(Serie serie)
    {
        Task<bool> result = _serie.actualizarSeries(serie);
        Console.WriteLine(" actualizado ");
        return Ok();
    }
}