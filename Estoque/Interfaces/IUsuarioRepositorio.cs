using Estoque.Models;

namespace Estoque.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Usuario? ValidarLogin(string email, string senha);
    }
}
