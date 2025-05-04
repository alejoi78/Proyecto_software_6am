using System;
using Proyecto_software_6am.DAOs.Interfaces;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Utilidades;
namespace Proyecto_software_6am.DAOs;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using BCrypt.Net;

public class UsuarioDAO : IUsuarioDAO
    {
    private readonly MySQLConfiguration _connectionString;

    public UsuarioDAO(MySQLConfiguration connectionString)
    {
        _connectionString = connectionString;
    }
    protected MySqlConnection dbConnection()
    {
        return _connectionString.dbConnection(); // Usa el método de la configuración
    }

    public async Task<List<Entidades.Usuario>> listarUsuarios()
        {
            List<Usuario> result = new List<Usuario>();
            string sql = " SELECT idUsuario,Nombre, Correo, idRol FROM prueba.usuario ";
            try
            {
                var db = dbConnection();
                IEnumerable<Usuario> lista = await db.QueryAsync<Usuario>(sql, new { });

                return lista.ToList(); ;
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error " + ex.Message);
            }
            return result;
        }

    public async Task<bool> guardarUsuarios(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));

        if (string.IsNullOrWhiteSpace(usuario.Nombre))
            throw new ArgumentException("El nombre es requerido");

        if (string.IsNullOrWhiteSpace(usuario.Correo))
            throw new ArgumentException("El correo es requerido");

        if (string.IsNullOrWhiteSpace(usuario.Contrasena))
            throw new ArgumentException("La contraseña es requerida");

        // Hashear la contraseña antes de almacenarla
        string hashedPassword = BCrypt.HashPassword(usuario.Contrasena);

        // FORZAR IdRol = 2 (usuario normal) sin importar lo que venga en el JSON
        usuario.IdRol = 2;

        using (var db = dbConnection())
        {
            await db.OpenAsync();

            string sql = @"INSERT INTO prueba.usuario
                     (nombre, correo, contrasena, idRol)
                     VALUES (@Nombre, @Correo, @Contrasena, @IdRol)";

            int result = await db.ExecuteAsync(sql, new
            {
                usuario.Nombre,
                usuario.Correo,
                Contrasena = hashedPassword,
                IdRol = 2 // Aseguramos que siempre sea 2
            });

            return result > 0;
        }
    }

    public async Task CrearAdminPorDefecto()
    {
        using (var db = dbConnection())
        {
            await db.OpenAsync();

            // Verificar si ya existe un admin (USANDO EL NOMBRE CORRECTO DE COLUMNA)
            var existeAdmin = await db.ExecuteScalarAsync<bool>(
                "SELECT 1 FROM prueba.usuario WHERE idRol = 1 LIMIT 1");

            if (!existeAdmin)
            {
                // Crear el admin por defecto
                await db.ExecuteAsync(
                    @"INSERT INTO prueba.usuario 
                (nombre, correo, contrasena, idRol) 
                VALUES (@Nombre, @Correo, @Contrasena, 1)",
                    new
                    {
                        Nombre = "Administrador",
                        Correo = "admin@example.com",
                        Contrasena = BCrypt.HashPassword("Admin123")
                    });

                    Console.WriteLine("Usuario admin creado exitosamente");
                }
            }
        } 
    

    public async Task<bool> actualizarUsuarios(Usuario usuario)
        {
            int result = 0;
            string sql = @"UPDATE usuario 
       SET nombre = @Nombre, 
           correo = @Correo, 
           contrasena = @Contrasena, 
           rol = @Rol
       WHERE idUsuario = @IdUsuario";

            try
            {
                using (var db = dbConnection())
                {
                    await db.OpenAsync();
                    result = await db.ExecuteAsync(sql, new
                    {
                        usuario.Nombre,
                        usuario.Correo,
                        usuario.Contrasena,
                        usuario.IdRol,
                        IdUsuario = usuario.IdUsuario
                    });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar usuario: {ex.Message}");
                return false;
            }
        }
    }
