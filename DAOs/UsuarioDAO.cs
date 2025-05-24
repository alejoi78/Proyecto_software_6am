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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

    public async Task<List<Usuario>> listarUsuarios()
    {
        string sql = @"
        SELECT u.*, r.TipoRol 
        FROM prueba.usuario u
        INNER JOIN prueba.rol r ON u.IdRol = r.IdRol";
        try
        {
            using (var db = dbConnection())
            {
                await db.OpenAsync();
                var lista = await db.QueryAsync<Usuario>(sql);
                return lista.ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al listar usuarios: {ex.Message}");
            return new List<Usuario>();
        }
    }

    public async Task<bool> guardarUsuarios(Usuario usuario)
    {
        using (var db = dbConnection())
        {
            await db.OpenAsync();

            using (var transaction = await db.BeginTransactionAsync())
            {
                try
                {
                    bool existeCorreo = await db.ExecuteScalarAsync<bool>(
                        "SELECT COUNT(1) FROM prueba.usuario WHERE correo = @Correo",
                        new { usuario.Correo },
                        transaction);

                    if (existeCorreo)
                    {
                        await transaction.RollbackAsync();
                        throw new ArgumentException("El correo ya está registrado");
                    }

                    string hashedPassword = BCrypt.HashPassword(usuario.Contrasena);
                    int result = await db.ExecuteAsync(
                        @"INSERT INTO prueba.usuario (nombre, correo, contrasena, idRol) 
                      VALUES (@Nombre, @Correo, @Contrasena, @IdRol)",
                        new
                        {
                            usuario.Nombre,
                            usuario.Correo,
                            Contrasena = hashedPassword,
                            usuario.IdRol
                        },
                        transaction);


                    await transaction.CommitAsync();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error en transacción: {ex.Message}");
                    throw;
                }
            }
        }
    }



    public async Task<object> Autenticar(Usuario usuario)
    {
        try
        {
            using (var db = dbConnection())
            {
                await db.OpenAsync();
                Console.WriteLine($"Consultando usuario: {usuario.Correo}");

                string sql = "SELECT * FROM prueba.usuario WHERE correo = @Correo LIMIT 1";
                var usuarioDB = await db.QueryFirstOrDefaultAsync<Usuario>(sql, new { usuario.Correo});

                Console.WriteLine($"Usuario encontrado: {(usuarioDB != null ? "Sí" : "No")}");

                if (usuarioDB != null)
                {
                    Console.WriteLine("Verificando contraseña...");
                    bool passwordMatch = BCrypt.Verify(usuario.Contrasena, usuarioDB.Contrasena);
                    Console.WriteLine($"Contraseña: {passwordMatch}");

                    if (passwordMatch)
                    {
                        // Usuario autenticado, generar token JWT
                        var secretKey = "tu_clave_secreta_muy_larga_y_segura_minimo_32_caracteres";
                        var key = Encoding.ASCII.GetBytes(secretKey);

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                            new Claim(ClaimTypes.NameIdentifier, usuarioDB.IdUsuario.ToString()),
                            new Claim(ClaimTypes.Name, usuarioDB.Nombre),
                            new Claim(ClaimTypes.Email, usuarioDB.Correo ?? ""),
                            new Claim("password", usuarioDB.Contrasena),
                            new Claim("idRol", usuarioDB.IdRol.ToString()),
                            }),
                            Expires = DateTime.UtcNow.AddHours(8),
                            SigningCredentials = new SigningCredentials(
                                new SymmetricSecurityKey(key),
                                SecurityAlgorithms.HmacSha256Signature
                            )
                        };

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var token = tokenHandler.CreateToken(tokenDescriptor);

                        return new
                        {
                            exito = true,
                            mensaje = "Autenticación exitosa",
                            token = tokenHandler.WriteToken(token),
                            usuario = new
                            {
                                id = usuarioDB.IdUsuario,
                                nombre = usuarioDB.Nombre,
                                correo = usuarioDB.Correo,
                                idRol = usuarioDB.IdRol
                            }
                        };
                    }

                }

                return new { exito = false, mensaje = "Correo o contraseña incorrectos" };
            }
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error en DAO de autenticación: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            return new { exito = false, mensaje = $"Error en el servidor: {ex.Message}" };
        }
    }



    public async Task<bool> actualizarUsuarios(Usuario usuario)
    {
        try
        {
            using (var db = dbConnection())
            {
                await db.OpenAsync();

                // Verificar si el correo existe en OTRO usuario (no en el actual)
                var correoPerteneceAOtroUsuario = await db.ExecuteAsync(
                    @"SELECT count(*) FROM prueba.usuario 
                  WHERE Correo = @Correo AND idUsuario <> @IdUsuario",
                    new { usuario.Correo, usuario.IdUsuario }
                );

                if (correoPerteneceAOtroUsuario > 0)
                {
                    Console.WriteLine(" Error: El correo ya está registrado por OTRO usuario.");
                    return false; // No permitir actualización
                }


                if (!string.IsNullOrWhiteSpace(usuario.Contrasena))
                {
                    usuario.Contrasena = BCrypt.HashPassword(usuario.Contrasena);
                    Console.WriteLine("Contraseña hasheada correctamente.");
                }
                string sql = @"UPDATE usuario 
                           SET nombre = @Nombre, 
                               correo = @Correo, 
                               contrasena = @Contrasena, 
                               idRol = @IdRol
                           WHERE idUsuario = @IdUsuario";

                int result = await db.ExecuteAsync(sql, new
                {
                    usuario.Nombre,
                    usuario.Correo,
                    usuario.Contrasena,
                    usuario.IdRol,
                    usuario.IdUsuario
                });

                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Error al actualizar usuario: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> eliminarUsuarios(int id)
    {
        int result = 0;
        string sql = "DELETE FROM prueba.usuario WHERE idusuario = @Idusuario";
        try
        {
            using (var db = dbConnection())
            {
                await db.OpenAsync();
                result = await db.ExecuteAsync(sql, new { idusuario = id });
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al eliminar: " + ex.Message);
            return false;
        }
    }
}

