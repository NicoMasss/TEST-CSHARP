using Dapper;
<<<<<<< Updated upstream
=======
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            string query = @"
            INSERT INTO users (name, email, password_hash)
            VALUES (@Name, @Email, @PasswordHash);";

            var result = conn.Connection.Execute(query, new
            {
                client.Name,
                client.Email,
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

        public List<ResponseClientJson> Get()
        {
            using var conn = new DBConnection();

            string query = "SELECT id, name, email FROM users WHERE is_deleted = false;";

            return conn.Connection.Query<ResponseClientJson>(query).ToList();
        }
        public List<RequestClientJson> Get()
        {
            using var conn = new DBConnection();

            string query = @"Select * FROM users;";

            var users = conn.Connection.Query<RequestClientJson>(sql: query);

            return users.ToList();
        }

        public ResponseClientJson GetByid(Guid id)
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

            string query = @"UPDATE users
                             SET is_deleted = TRUE
                                 is_deleted = true 
                                    WHERE id = @id";

            var result = conn.Connection.Execute(query, new { Id = id });

            return result == 1;
        }

        //public bool Update(Guid id, RequestClientJson client) 
        //{
        //    using var conn = new DBConnection();

        //    var passwordHash = BCrypt.Net.BCrypt.HashPassword(client.Password);

        //    string query = string query = @"UPDATE users
        //                                    SET name = @Name,
        //                                        email = @Email,
        //                                        password_hash = @PasswordHash,
        //                                        updated_at = CURRENT_TIMESTAMP
        //                                            WHERE id = @Id;";

        //    var result = conn.Connection.Execute(query, new
        //    {
        //        client.Name,
        //        client.Email,
        //        PasswordHash = passwordHash,
        //        Id = id
        //    });

        //    return result == 1;
        //}
    }
}
