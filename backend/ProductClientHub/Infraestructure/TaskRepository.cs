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

            var query = @"INSERT INTO public.tasks(title, description, status, assigned_to, due_date, assigned_to_clients) VALUES (@Title, @Description, @Status, @AssignedTo, @DueDate, @AssignedToClient);"; // VER AS VARIAVEIS ESTAO CERTAS

            var result = conn.Connection.Execute(sql: query, param: new
            {
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                AssignedTo = task.AssignedTo,
                AssignedToClient = task.AssignedToClient,
                DueDate = task.DueDate
            });

            return result == 1;
        }

        public List<TaskResponse> Get() 
        {
            using var conn = new DBConnection();

            string query = @"SELECT id, title, description, status, assigned_to AS AssignedTo, due_date AS DueDate, assigned_to_clients AS AssignedToCLient FROM tasks WHERE is_deleted = false;";

            var tasks = conn.Connection.Query<TaskResponse>(sql: query);

            return tasks.ToList();
        }

        public TaskResponse GetById(Guid id)
        {
            using var conn = new DBConnection();

            var query = @"SELECT id, id, title, description, status, assigned_to AS AssignedTo, due_date AS DueDate, assigned_to_clients AS AssignedToCLient
                            FROM tasks 
                                WHERE id = @Id AND is_deleted = false;";

            var result = conn.Connection.QueryFirstOrDefault<TaskResponse>(
                sql: query,
                param: new { Id = id }
            );

            return result;
        }

        public List<TaskResponse> GetByAssignedTo(Guid id)
        {
            using var conn = new DBConnection();

            var query = @"SELECT id, title, description, status, assigned_to AS AssignedTo, due_date AS DueDate, assigned_to_clients AS AssignedToCLient
                            FROM tasks 
                                WHERE assigned_to = @Id AND is_deleted = false;";

            var client = conn.Connection.Query<TaskResponse>(query, new { Id = id });

            return client.ToList();
        }

        public List<TaskResponse> GetByAssignedToClient(Guid id)
        {
            using var conn = new DBConnection();

            var query = @"SELECT id, title, description, status, assigned_to AS AssignedTo, due_date AS DueDate, assigned_to_clients AS AssignedToCLient
                            FROM tasks 
                                WHERE assigned_to_clients = @Id AND is_deleted = false;";

            var client = conn.Connection.Query<TaskResponse>(query, new { Id = id });

            return client.ToList();
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

            if (task.NewDueDate.HasValue)
            {
                fieldsToUpdate.Add("due_date = @DueDate");
                parameters.Add("DueDate", task.NewDueDate.Value);
            }

            if (task.NewAssignedTo.HasValue)
            {
                fieldsToUpdate.Add("assigned_to = @AssignedTo");
                parameters.Add("AssignedTo", task.NewAssignedTo.Value);
            }

            if (task.NewAssignedToClient.HasValue)
            {
                fieldsToUpdate.Add("assigned_to_clients = @AssignedToClients");
                parameters.Add("AssignedToClients", task.NewAssignedToClient.Value);
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

        public List<TaskResponse> GetTasksByStatusAndDueDate(string status, DateTime? dueDate)
        {
            using var conn = new DBConnection();

            var sql = @"SELECT id, title, description, status, assigned_to AS AssignedTo, due_date AS DueDate, assigned_to_clients AS AssignedToCLient FROM tasks WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(status))
            {
                sql += " AND status = @Status";
                parameters.Add("@Status", status);
            }

            if (dueDate.HasValue)
            {
                sql += " AND DATE(due_date) <= @DueDate";
                parameters.Add("@DueDate", dueDate.Value.Date);
            }

            var result =  conn.Connection.Query<TaskResponse>(sql, parameters);

            return result.ToList();
        }
    }
}
