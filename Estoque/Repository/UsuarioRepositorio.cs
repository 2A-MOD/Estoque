using Estoque.Models;
using Estoque.Interfaces;
using MySql.Data.MySqlClient;

namespace Estoque.Repository
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        // criando variável que irá armazenar a string de conexao
        private readonly string _connectionString;
        // construtor que que busca e armazena a string de conexao
        public UsuarioRepositorio(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Conexao")!;
        }
        public Usuario? ValidarLogin(string email, string senha)
        {
            // estabelece a conexao com o banco de dados
            using var conn = new MySqlConnection(_connectionString);
            // abre a conexao com o banco de dados
            conn.Open();

            var sql = "select * from Usuario where Email = @email and Senha = @senha";
            var cmd = new MySqlCommand(sql, conn);
            // passa o email e senha com parametros para serem entendidos somente como texto
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@senha", senha);

            using var reader = cmd.ExecuteReader();
            // caso consiga ler
            if (reader.Read())
            {
                // retorna os dados do usuario e faz suas conversoes
                return new Usuario
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    NivelAcesso = reader["NivelAcesso"].ToString()!
                };
            }
            return null;
        }
    }
}

