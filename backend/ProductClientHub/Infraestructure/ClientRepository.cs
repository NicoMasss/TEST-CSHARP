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

            string query = @"INSERT INTO public.clients(
	                            name, email, description)
	                                VALUES (@Name, @Email, @Description);"; 

            var result = conn.Connection.Execute(sql: query, param: new
            {
                Name = client.Name,
                Email = client.Email,
                Description = client.Description
            });

            return result == 1;
        }

        public List<ResponseClientJson> Get() 
        {
            using var conn = new DBConnection();

            string query = @"SELECT name, email, id FROM clients WHERE is_deleted = false;";

            var clients = conn.Connection.Query<ResponseClientJson>(sql: query);

            return clients.ToList();
        }

        public ResponseClientJson GetById(Guid id)
        {
            using var conn = new DBConnection();

            string query = @"SELECT id, name, email 
                                FROM clients 
                                    WHERE id = @Id AND is_deleted = false;";

            var client = conn.Connection.QueryFirstOrDefault<ResponseClientJson>(query, new { Id = id });

            return client;
        }

        public bool Delete(Guid id)
        {
            using var conn = new DBConnection();

            string query = "UPDATE clients SET is_deleted = true WHERE id = @Id;";

            var result = conn.Connection.Execute(query, new { Id = id });

            return result == 1;
        }

        public bool Update(RequestClientJsonUpdate client)
        {
            using var conn = new DBConnection();

            var fieldsToUpdate = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(client.NewName))
            {
                fieldsToUpdate.Add("name = @Name");
                parameters.Add("Name", client.NewName);
            }

            if (!string.IsNullOrEmpty(client.NewEmail))
            {
                fieldsToUpdate.Add("email = @Email");
                parameters.Add("Email", client.NewEmail);
            }

            if (!string.IsNullOrEmpty(client.NewDescription))
            {
                fieldsToUpdate.Add("description = @Description");
                parameters.Add("Description", client.NewDescription);
            }

            if (!fieldsToUpdate.Any())
                return false;

            parameters.Add("Id", client.Id);

            var query = $@"
        UPDATE clients
        SET {string.Join(", ", fieldsToUpdate)}
        WHERE id = @Id AND is_deleted = false";

            var success = conn.Connection.Execute(query, parameters);

            return success == 1;
        }

    }
}