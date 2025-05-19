using Proyecto_software_6am.Entidades;

namespace Proyecto_software_6am.Servicio.Interfaces
{
    public interface IUsuarioNegocio
    {
        Task<List<Entidades.Usuario>> listarUsuarios();
        Task<Boolean> guardarUsuarios(Usuario usuario);
        Task<Boolean> actualizarUsuarios(Usuario usuario);
        Task<object> Autenticar(Usuario usuario);
        Task CrearAdminPorDefecto();
    }
}
