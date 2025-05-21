using Proyecto_software_6am.Entidades;

namespace Proyecto_software_6am.DAOs.Interfaces

{
    public interface IUsuarioDAO
    {
        Task<List<Entidades.Usuario>> listarUsuarios();
        Task<Boolean> guardarUsuarios(Usuario usuario);
        Task<Boolean> actualizarUsuarios(Usuario usuario);
        Task<Boolean> eliminarUsuarios(int id);
        Task<object> Autenticar(Usuario usuario);
       
    }
}
