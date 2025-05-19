using Microsoft.AspNetCore.Mvc;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Servicio;
using Proyecto_software_6am.Servicio.Interfaces;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proyecto_software_6am.Controllers;

[ApiController]
[Route("usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioNegocio _usuario;

    public UsuarioController(IUsuarioNegocio usuario)
    {
        _usuario = usuario;
    }

    [HttpGet]
    [Route("listar")]
    public async Task<IActionResult> listarUsuarios()
    {
        try
        {
            var usuarios = await _usuario.listarUsuarios();

            // Debug: Mostrar datos obtenidos
            Console.WriteLine($"Datos obtenidos: {JsonSerializer.Serialize(usuarios)}");

            if (usuarios == null || !usuarios.Any())
                return NotFound("No se encontraron usuarios");

            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GET /api/usuarios/listar: {ex.Message}");
            return StatusCode(500, "Error al obtener los usuarios");
        }
    }

    [HttpPost]
    [Route("registrar")]
    public async Task<IActionResult> Post([FromBody] Usuario usuario)
    {
        try
        {
            // Validaciones básicas
            if (usuario == null)
                return BadRequest("Datos de usuario inválidos");

            if (string.IsNullOrEmpty(usuario.Nombre))
                return BadRequest("El nombre es requerido");

            if (string.IsNullOrEmpty(usuario.Correo))
                return BadRequest("El correo es requerido");

            if (string.IsNullOrEmpty(usuario.Contrasena))
                return BadRequest("La contraseña es requerida");


            Console.WriteLine($"Datos a guardar: {JsonSerializer.Serialize(usuario)}");

            bool resultado = await _usuario.guardarUsuarios(usuario);

            if (!resultado)
                return StatusCode(500, "Error al registrar el usuario");

            return Ok(new
            {
                success = true,
                message = "Usuario registrado correctamente",
                data = new { usuario.Nombre, usuario.Correo }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en registro: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut]
    [Route("actualizar")]
    public async Task<IActionResult> actualizarUsuarios(Usuario usuario)
    {
        try
        {
            if (usuario == null)
                return BadRequest("Datos inválidos");

            if (usuario.IdUsuario <= 0)
                return BadRequest("ID de usuario inválido");

            Console.WriteLine($"Datos recibidos: {JsonSerializer.Serialize(usuario)}");

            bool resultado = await _usuario.actualizarUsuarios(usuario);

            if (!resultado)
            {
                Console.WriteLine(" No se pudo actualizar: Correo en uso por otro usuario.");
                return BadRequest("El correo ya está registrado por otro usuario.");
            }

            return Ok("Usuario actualizado correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error en actualización: {ex.Message}");
            return StatusCode(500, "Error interno del servidor.");
        }
    }



    [HttpPost]
    [Route("autenticar")]
    public async Task<IActionResult> Autenticar([FromBody] Usuario usuario)
    {
        try
        {

            if (string.IsNullOrEmpty(usuario.Correo))
                return BadRequest("El correo del usuario es requerido");

            if (string.IsNullOrEmpty(usuario.Contrasena))
                return BadRequest("La contraseña es requerida");

            Console.WriteLine($"Intentando autenticar usuario: {usuario.Correo}");

            // Llamada directa al servicio
            var resultado = await _usuario.Autenticar(usuario);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en autenticación: {ex.Message}");
            return StatusCode(500, new { exito = true, mensaje = ex.Message });
        }
    }



}