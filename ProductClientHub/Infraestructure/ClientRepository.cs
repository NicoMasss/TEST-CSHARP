using Dapper;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Infraestructure
{
    public class ClientRepository
    {
        public bool Add(RequestClientJson client)
        {
            using var conn = new DBConnection();

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(client.Password);

            string query = @"
            INSERT INTO users (name, email, password_hash)
            VALUES (@Name, @Email, @PasswordHash);";

            var result = conn.Connection.Execute(query, new
            {
                client.Name,
                client.Email,
                PasswordHash = passwordHash
            });

            return result == 1;
        }

        public List<ResponseClientJson> Get()
        {
            using var conn = new DBConnection();

            string query = "SELECT id, name, email FROM users WHERE is_deleted = false;";

            return conn.Connection.Query<ResponseClientJson>(query).ToList();
        }
    }
}
