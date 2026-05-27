using Estoque.Interfaces;
using Estoque.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Estoque.Controllers
{
    public class UsuarioController : Controller
    {
        // Injeção de dependência do repositório de usuários
        private readonly IUsuarioRepositorio _userRepo;
        // Construtor para receber o repositório via injeção de dependência
        public UsuarioController(IUsuarioRepositorio userRepo)
        {   // Atribui o repositório recebido ao campo privado para uso nos métodos do controlador
            _userRepo = userRepo;
        }
        // Método para exibir a página de login
        [HttpGet]
        // Retorna a view de login para o usuário
        public IActionResult Login() => View();
        // Método para processar os dados de login enviados pelo usuário
        [HttpPost]
        // Valida o token antifalsificação para proteger contra ataques CSRF
        [ValidateAntiForgeryToken]
        // Recebe um modelo de usuário contendo os dados de login (email e senha)
        public async Task<IActionResult> Login(Usuario model)
        {
            // Verifica se o modelo de dados é válido (todos os campos obrigatórios foram preenchidos corretamente)
            if (!ModelState.IsValid) return View(model);
            // Chama o método de validação do repositório de usuários para verificar se as credenciais são corretas
            var user = _userRepo.ValidarLogin(model.Email, model.Senha);
            // Se o usuário for encontrado e as credenciais forem válidas, cria uma lista de claims para armazenar informações do usuário
            if (user != null)
            {
                // Cria uma lista de claims que inclui o nome, email, nível de acesso e ID do usuário
                var claims = new List<Claim>
                {
                    // Adiciona um claim para o nome do usuário
                    new Claim(ClaimTypes.Name, user.Nome),
                    // Adiciona um claim para o email do usuário
                    new Claim(ClaimTypes.Email, user.Email),
                    // Adiciona um claim personalizado para o nível de acesso do usuário
                    new Claim("NivelAcesso", user.NivelAcesso),
                    // Adiciona um claim personalizado para o ID do usuário, convertendo-o para string
                    new Claim("UsuarioId", user.Id.ToString())
                };
                // Cria uma identidade de claims usando o esquema de autenticação de cookies
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                // Realiza o processo de autenticação, criando um cookie de autenticação para o usuário
                await HttpContext.SignInAsync(
                    // Esquema de autenticação utilizado (neste caso, cookies)
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    // Cria um principal de claims a partir da identidade de claims criada
                    new ClaimsPrincipal(claimIdentity),
                    // Define as propriedades de autenticação, como a persistência da sessão (neste caso, não persistente)
                    new AuthenticationProperties { IsPersistent = false });
                // Redireciona o usuário para a página inicial após um login bem-sucedido
                return RedirectToAction("Index", "Home");
            }
            // Se o usuário não for encontrado ou as credenciais forem inválidas, adiciona um erro ao modelo
            ModelState.AddModelError(string.Empty, "Email ou senha inválidos.");
            // Se as credenciais forem inválidas, adiciona um erro ao modelo e retorna a view de login para o usuário tentar novamente
            return View(model);

        }
        // Método para processar a ação de logout do usuário
        public async Task<IActionResult> Sair()
        {
            // Encerra a sessão do usuário, removendo os cookies de autenticação
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Redireciona o usuário para a página de login após sair
            return RedirectToAction("Login");
        }
    }
}
