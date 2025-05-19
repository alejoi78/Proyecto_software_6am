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

            // Hashear la contraseña y asignar rol por defecto
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);
            usuario.IdRol = 2; // Rol de usuario normal

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
    public IActionResult actualizarUsuarios(Usuario usuario)
    {
        try
        {
            if (usuario == null)
                return BadRequest("Datos de usuario inválidos");

            if (usuario.IdUsuario <= 0)
                return BadRequest("ID de usuario inválido");

            Console.WriteLine($"Datos recibidos para actualizar: {JsonSerializer.Serialize(usuario)}");

            Task<bool> result = _usuario.actualizarUsuarios(usuario);
            Console.WriteLine(" actualizado ");
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en PUT /api/usuarios/actualizar: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

   

    [HttpPost]
    [Route("autenticar")]
    public async Task<IActionResult> Autenticar([FromBody] Usuario usuario)
    {
        try
        {
            // Validaciones básicas
            if (usuario == null)
                return BadRequest("Datos de usuario inválidos");

            if (string.IsNullOrEmpty(usuario.Nombre))
                return BadRequest("El nombre de usuario es requerido");

            if (string.IsNullOrEmpty(usuario.Contrasena))
                return BadRequest("La contraseña es requerida");

            Console.WriteLine($"Intentando autenticar usuario: {usuario.Nombre}");

            // Llamada directa al servicio
            var resultado = await _usuario.Autenticar(usuario);

            // Devolver el resultado tal cual viene del servicio
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en autenticación: {ex.Message}");
            return StatusCode(500, new { exito = true, mensaje = ex.Message });
        }
    }



}