using Dapper;
using ProductClientHub.Communication.Requests;

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

            string query = @"Select * FROM users;";

            var users = conn.Connection.Query<RequestClientJson>(sql: query);

            return users.ToList();
        }
    }
}
