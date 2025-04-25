using Microsoft.AspNetCore.Mvc;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Servicio;
using Proyecto_software_6am.Servicio.Interfaces;
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
    public Task<List<Serie>> Get()
    {
        return _serie.listarSeries();
    }

    [HttpPost]
    [Route("nuevo")]
    public IActionResult Post(Serie serie)
    {
        Task<bool> result = _serie.guardarSeries(serie);
        if (result.IsCompleted)
        {
        }
        Console.WriteLine(" insertado ");
        return Ok();
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

