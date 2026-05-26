using Estoque.Models;

namespace Estoque.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Usuario? Validar(string email, string senha);
    }
}
