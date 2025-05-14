using Dapper;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.API.Infraestructure;

namespace ProductClientHub.API.Repositories
{
    public class TaskRepository
    {
        public Guid Add(TaskRequest task)
        {
            using var conn = new DBConnection();

            string query = @"
                INSERT INTO tasks (title, description, status, assigned_to)
                VALUES (@Title, @Description, @Status, @AssignedTo)
                RETURNING id;";

            var id = conn.Connection.ExecuteScalar<Guid>(query, new
            {
                task.Title,
                task.Description,
                task.Status,
                task.AssignedTo
            });

            return id;
        }

        public TaskResponse? GetById(Guid id)
        {
            using var conn = new DBConnection();

            string query = @"
                SELECT id, title, description, status, assigned_to
                FROM tasks
                WHERE id = @Id;";

            return conn.Connection.QueryFirstOrDefault<TaskResponse>(query, new { Id = id });
        }

        public List<TaskResponse> GetByUser(Guid userId)
        {
            using var conn = new DBConnection();

            string query = @"
                SELECT id, title, description, status, assigned_to
                FROM tasks
                WHERE assigned_to = @UserId;";

            var tasks = conn.Connection.Query<TaskResponse>(query, new { UserId = userId });
            return tasks.ToList();
        }

        public bool Update(Guid id, TaskRequest task)
        {
            using var conn = new DBConnection();

            string query = @"
                UPDATE tasks
                SET title = @Title,
                    description = @Description,
                    status = @Status,
                    assigned_to = @AssignedTo,
                    updated_at = CURRENT_TIMESTAMP
                WHERE id = @Id;";

            var result = conn.Connection.Execute(query, new
            {
                task.Title,
                task.Description,
                task.Status,
                task.AssignedTo,
                Id = id
            });

            return result == 1;
        }

        public bool Delete(Guid id)
        {
            using var conn = new DBConnection();

            string query = "DELETE FROM tasks WHERE id = @Id;";
            var result = conn.Connection.Execute(query, new { Id = id });

            return result == 1;
        }
    }
}
