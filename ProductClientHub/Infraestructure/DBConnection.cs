using Npgsql;

namespace ProductClientHub.API.Infraestructure
{
    public class DBConnection : IDisposable
    {
        public NpgsqlConnection Connection { get; set; }

        public DBConnection() 
        {
            Connection = new NpgsqlConnection("Server=localhost;Port=5432;Database=postgres;User id=postgres;Password=123");
            Connection.Open(); 
        }

        public void Dispose() 
        {
            Connection.Dispose();
        }
    }
}
