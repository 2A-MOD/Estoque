using Estoque.Interfaces;
using Estoque.Models;
using EstoqueLoja.Repository;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet]
        // Método para exibir o formulário de edição de um produto específico
        public IActionResult Editar(int id)
        {
            // Chama o método do repositório para buscar o produto pelo ID e armazena em uma variável
            var product = _productRepo.BuscarPorId(id);
            // Verifica se o produto existe, caso contrário, retorna um resultado de "NotFound" para indicar que o recurso não foi encontrado
            if (product == null) return NotFound();

            var viewModel = new Produto
            {
                Id = product.Id,
                Nome = product.Nome,
                Preco = product.Preco
            };
            // Retorna a view "Editar" passando o viewModel como modelo para preencher os campos do formulário de edição
            return View(viewModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Método para processar os dados enviados pelo formulário de edição de produto
        public IActionResult Editar(int id, Produto model)
        {
            // Verifica se o ID do produto no modelo corresponde ao ID fornecido como parâmetro, caso contrário, retorna um resultado de "BadRequest" para indicar que a solicitação é inválida
            if (id != model.Id) return BadRequest();
            
            if (ModelState.IsValid)
            {
                var product = new Produto
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    Preco = model.Preco
                };


                _productRepo.Atualizar(product);

                return RedirectToAction(nameof(Index));
            }
            
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        // Método para processar a exclusão de um produto específico
        public IActionResult Excluir(int id)
        {
            _productRepo.Excluir(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
