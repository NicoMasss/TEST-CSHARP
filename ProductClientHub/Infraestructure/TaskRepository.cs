using System.Security.Cryptography.X509Certificates;
using Dapper;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.API.Infraestructure
{
    public class TaskRepository
    {
        public bool Add(TaskRequest task)
        {
            using var conn = new DBConnection();

            var query = @"INSERT INTO public.tasks(
	                            title, description, status, assigned_to)
	                                VALUES (@Title, @Description, @Status, @AssignedTo);"; // VER AS VARIAVEIS ESTAO CERTAS

            var result = conn.Connection.Execute(sql: query, param: new
            {
                task.Title,
                task.Description,
                task.Status,
                task.AssignedTo

            });

            return result == 1;
        }

        public TaskRequest GetById(Guid id)
        {
            using var conn = new DBConnection();

            var query = @"SELECT title, description, status, assigned_to
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

            var query = @"SELECT title, description, status, assigned_to
                            FROM tasks 
                                WHERE assigned_to = @Id AND is_deleted = false;";

            var result = conn.Connection.Execute(sql: query, param: new
            {
                task.Title,
                task.Description,
                task.Status,
                task.AssignedTo

            });

            return result;
        }

        public bool Delete(Guid id)
        {
            using var conn = new DBConnection();

            var query = @"DELETE FROM Tasks
                            WHERE id = @id AND is_deleted = false;";

            var result = conn.Connection.Execute(query, new { Id = id });

            return result == 1;
        }
    }
}
