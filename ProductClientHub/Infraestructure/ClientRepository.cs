using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
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

            string query = @"INSERT INTO public.users(
	                            name, email, password_hash)
	                                VALUES (@Name, @Email, @passwordHash);"; 

            var result = conn.Connection.Execute(sql: query, param: new
            {
                Name = client.Name,
                Email = client.Email,
                PasswordHash = passwordHash
            });

            return result == 1;
        }

        public List<RequestClientJson> Get() 
        {
            using var conn = new DBConnection();

            string query = @"SELECT name, email FROM users WHERE is_deleted = false;";

            var users = conn.Connection.Query<RequestClientJson>(sql: query);

            return users.ToList();
        }

        public ResponseClientJson GetById(Guid id)
        {
            using var conn = new DBConnection();

            string query = @"SELECT id, name, email 
                                FROM users 
                                    WHERE id = @Id AND is_deleted = false;";

            var client = conn.Connection.QueryFirstOrDefault<ResponseClientJson>(query, new { Id = id });

            return client;
        }

        public bool Delete(Guid id)
        {
            using var conn = new DBConnection();

            string query = "UPDATE users SET is_deleted = true WHERE id = @Id;";

            var result = conn.Connection.Execute(query, new { Id = id });

            return result == 1;
        }

        public bool Update(RequestClientJsonUpdate client) 
        {
            using var conn = new DBConnection();

            var passwordQuery = @"SELECT password_hash FROM users 
                                    WHERE email = @Email AND is_deleted = false;";

            var storedPasswordHash = conn.Connection.QueryFirstOrDefault<string>(passwordQuery, new { Email = client.Email });

            if (storedPasswordHash is null) return false;

            bool passwordMatches = BCrypt.Net.BCrypt.Verify(client.Password, storedPasswordHash);

            if (!passwordMatches) return false; //tem que retornar senha incorreta

            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(client.NewPassword);

            var updateQuery = @"UPDATE users 
                                            SET password_hash = @NewPasswordHash
                                                    WHERE email = @Email AND is_deleted = false;";

            var result = conn.Connection.Execute(updateQuery, new
            {
                NewPasswordHash = newPasswordHash,
                Email = client.Email
            });

            return result == 1;
        }
 
        public (RequestClientJson? user, string? passwordHash, Guid Id) GetByEmail(string email)
        {
            using var conn = new DBConnection();

            var passwordQuery = @"SELECT password_hash FROM users 
                                    WHERE email = @Email AND is_deleted = false;";

            var storedPasswordHash = conn.Connection.QueryFirstOrDefault<string>(passwordQuery, new { Email = email });

            var IdQuery = @"SELECT id FROM users 
                                    WHERE email = @Email AND is_deleted = false;";

            Guid storedId = conn.Connection.QueryFirstOrDefault<Guid>(IdQuery, new { Email = email });

            string query = "SELECT * FROM users WHERE email = @Email AND is_deleted = false";

            var user = conn.Connection.QueryFirstOrDefault<RequestClientJson>(query, new { Email = email });

            return (user, storedPasswordHash, storedId);
        }
    }
}