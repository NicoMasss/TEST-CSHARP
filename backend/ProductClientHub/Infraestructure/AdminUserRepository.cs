using Dapper;
using Npgsql;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Infraestructure
{
    public class UserAdminRepository
    {
        public bool Add(AuthRequestRegister admin)
        {
            using var conn = new DBConnection();

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(admin.Password);

            string query = @"INSERT INTO public.admin_users
                             (name, email, password_hash)
                             VALUES (@Name, @Email, @PasswordHash);";

            var result = conn.Connection.Execute(query, new
            {
                Name = admin.Name,
                Email = admin.Email,
                PasswordHash = passwordHash
            });

            return result == 1;
        }

        public List<UserResponse> Get() 
        {
            using var conn = new DBConnection();

            string query = @"SELECT name, email, id FROM admin_users WHERE is_deleted = false;";

            var user = conn.Connection.Query<UserResponse>(sql: query);

            return user.ToList();
        }

        public (AuthRequestLogin? user, string? passwordHash, Guid Id) GetByEmail(string email)
        {
            using var conn = new DBConnection();

            var passwordQuery = @"SELECT password_hash FROM admin_users 
                                    WHERE email = @Email";

            var storedPasswordHash = conn.Connection.QueryFirstOrDefault<string>(passwordQuery, new { Email = email });

            var IdQuery = @"SELECT id FROM admin_users 
                                    WHERE email = @Email";

            Guid storedId = conn.Connection.QueryFirstOrDefault<Guid>(IdQuery, new { Email = email });

            string query = "SELECT * FROM admin_users WHERE email = @Email";

            var user = conn.Connection.QueryFirstOrDefault<AuthRequestLogin>(query, new { Email = email });

            return (user, storedPasswordHash, storedId);
        }

        public void SaveGoogleAccessToken(string email, string accessToken)
        {
            using var conn = new DBConnection();

            var sql = "UPDATE admin_users SET google_access_token = @Token WHERE email = @Email";

            conn.Connection.Execute(sql, new { Token = accessToken, Email = email });
        }

        public string? GetGoogleAccessToken(string email)
        {
            using var conn = new DBConnection();

            var sql = "SELECT google_access_token FROM admin_users WHERE email = @Email";

            return conn.Connection.QueryFirstOrDefault<string>(sql, new { Email = email });
        }

    }
}
