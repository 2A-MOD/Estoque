using Estoque.Models;
using Estoque.Interfaces;
using MySql.Data.MySqlClient;

namespace EstoqueLoja.Repository
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly string _connectionString;

        public ProdutoRepositorio(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Conexao")!;
        }
        public void Adicionar(Produto produto)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var sql = "insert into Produto (Nome, Preco) values (@nome, @preco)";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", produto.Nome);
            cmd.Parameters.AddWithValue("@preco", produto.Preco);
            // executa o comando sql sem retornar dados
            cmd.ExecuteNonQuery();
        }
        public void Atualizar(Produto produto)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var sql = "update Produto set Nome = @nome, Preco = @preco where Id = @id";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", produto.Nome);
            cmd.Parameters.AddWithValue("@preco", produto.Preco);
            cmd.Parameters.AddWithValue("@id", produto.Id);
            cmd.ExecuteNonQuery();
        }
        public void Excluir(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var sql = "delete from Produto where Id = @id";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        public IEnumerable<Produto> ListarProdutos()
        {
            var list = new List<Produto>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var sql = "select * from Produto";
            var cmd = new MySqlCommand(sql, conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Produto
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString()!,
                    Preco = Convert.ToDecimal(reader["Preco"])
                });
            }
            return list;
        }
        public Produto? BuscarPorId(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var sql = "select * from Produto where Id = @id";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Produto
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString()!,
                    Preco = Convert.ToDecimal(reader["Preco"])
                };
            }
            return null;
        }
    }
}
