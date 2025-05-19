using Proyecto_software_6am.DAOs.Interfaces;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Servicio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_software_6am.Servicio
{
    public class UsuarioNegocio : IUsuarioNegocio
    {
        private readonly IUsuarioDAO _usuarioDAO;

        public UsuarioNegocio(IUsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        public async Task<List<Usuario>> listarUsuarios()
        {
            return await _usuarioDAO.listarUsuarios();
        }

        public async Task<bool> guardarUsuarios(Usuario usuario)
        {
            // Validaciones básicas antes de guardar
            if (string.IsNullOrWhiteSpace(usuario.Correo))
                throw new ArgumentException("El correo no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(usuario.Contrasena))
                throw new ArgumentException("La contraseña no puede estar vacía.");
            return await _usuarioDAO.guardarUsuarios(usuario);
        }

        public async Task<bool> actualizarUsuarios(Usuario usuario)
        {
            if (usuario.IdUsuario <= 0)
                throw new ArgumentException("ID de usuario inválido.");
            return await _usuarioDAO.actualizarUsuarios(usuario);
        }

        public async Task<object> Autenticar(Usuario usuario)
        {
            // Validaciones básicas antes de autenticar
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(usuario.Contrasena))
                throw new ArgumentException("La contraseña no puede estar vacía.");
            return await _usuarioDAO.Autenticar(usuario);
        }

        // Nuevo método para inicialización del admin
        public async Task CrearAdminPorDefecto()
        {
            await _usuarioDAO.CrearAdminPorDefecto();
        }
    }
}