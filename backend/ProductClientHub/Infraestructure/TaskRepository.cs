using System.Security.Cryptography.X509Certificates;
using Dapper;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Infraestructure
{
    public class TaskRepository
    {
        public bool Add(TaskRequest task)
        {
            using var conn = new DBConnection();

            var query = @"INSERT INTO public.tasks(title, description, status, assigned_to) VALUES (@Title, @Description, @Status, @AssignedTo);"; // VER AS VARIAVEIS ESTAO CERTAS

            var result = conn.Connection.Execute(sql: query, param: new
            {
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                AssignedTo = task.AssignedTo

            });

            return result == 1;
        }

        public TaskRequest GetById(Guid id)
        {
            using var conn = new DBConnection();

            var query = @"SELECT title, description, status, assigned_to AS AssignedTo
                            FROM tasks 
                                WHERE id = @Id AND is_deleted = false;";

            var result = conn.Connection.QueryFirstOrDefault<TaskRequest>(
                sql: query,
                param: new { Id = id }
            );

            return result;
        }

        public TaskRequest GetByAssignedTo(Guid id)
        {
            using var conn = new DBConnection();

            var query = @"SELECT title, description, status, assigned_to AS AssignedTo
                            FROM tasks 
                                WHERE assigned_to = @Id AND is_deleted = false;";

            var client = conn.Connection.QueryFirstOrDefault<TaskRequest>(query, new { Id = id });

            return client;
        }

        public bool Update(TaskRequestUpdate task)
        {

            using var conn = new DBConnection();

            var fieldsToUpdate = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(task.NewTitle))
            {
                fieldsToUpdate.Add("title = @Title");
                parameters.Add("Title", task.NewTitle);
            }

            if (!string.IsNullOrEmpty(task.NewDescription))
            {
                fieldsToUpdate.Add("description = @Description");
                parameters.Add("Description", task.NewDescription);
            }

            if (!string.IsNullOrEmpty(task.NewStatus))
            {
                fieldsToUpdate.Add("status = @Status");
                parameters.Add("Status", task.NewStatus);
            }

            if (!fieldsToUpdate.Any())
                return false;

            parameters.Add("Id", task.Id);

            var query = $@"UPDATE tasks
                                SET {string.Join(", ", fieldsToUpdate)}
                                    WHERE id = @Id AND is_deleted = false";

            var success = conn.Connection.Execute(query, parameters);

            return success == 1;
        }

        public bool Delete(Guid id)
        {
            using var conn = new DBConnection();

            var query = @"UPDATE tasks 
                                SET is_deleted = true
                                    WHERE id = @Id;";

            var result = conn.Connection.Execute(query, new { Id = id });

            return result == 1;
        }
    }
}
