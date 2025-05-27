using Dapper;
using ProductClientHub.Communication.Requests;

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
    }
}
