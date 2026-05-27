using Estoque.Interfaces;
using Estoque.Models;
using EstoqueLoja.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _productRepo;
        public ProdutoController(IProdutoRepositorio productRepo)
        {
            _productRepo = productRepo;
        }
        // Método para exibir a lista de produtos
        public IActionResult Index()
        {
            // Chama o método do repositório para obter a lista de produtos e armazena em uma variável
            var products = _productRepo.ListarProdutos();
            // Retorna a view "Index" passando a lista de produtos como modelo para ser exibida na interface do usuário
            return View(products);
        }
        [HttpGet]
        public IActionResult Criar() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Método para processar os dados enviados pelo formulário de criação de produto
        public IActionResult Criar(Produto model)
        {
            // Verifica se os dados do modelo são válidos, caso contrário, retorna a mesma view para que o usuário possa corrigir os erros
            if (!ModelState.IsValid) return View(model);

            // Cria um novo objeto Produto com os dados recebidos do modelo
            var product = new Produto
            {
                Nome = model.Nome,
                Preco = model.Preco
            };
            // Chama o método do repositório para adicionar o novo produto à lista de produtos
            _productRepo.Adicionar(product);
            // Redireciona o usuário para a ação "Index" para exibir a lista atualizada de produtos
            return RedirectToAction(nameof(Index));
        }
    }
}
