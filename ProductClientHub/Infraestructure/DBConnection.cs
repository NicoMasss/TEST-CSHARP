<<<<<<< Updated upstream
﻿using Npgsql;
=======
﻿using System.Data;
using Npgsql;
>>>>>>> Stashed changes

namespace ProductClientHub.API.Infraestructure
{
    public class DBConnection : IDisposable
    {
<<<<<<< Updated upstream
        public NpgsqlConnection Connection { get; set; }

        public DBConnection() 
        {
            Connection = new NpgsqlConnection("Server=localhost;Port=5432;Database=Project_Engenharia;User id=postgres;Password=123");
            Connection.Open(); 
        }

        public void Dispose() 
=======
        public IDbConnection Connection { get; }

        public DBConnection()
        {
            var connectionString = "Host=localhost;Port=5432;User Id=postgres;Password=123;Database=Project_Engenharia;";

            Connection = new NpgsqlConnection(connectionString);

            Connection.Open();
        }

        public void Dispose()
>>>>>>> Stashed changes
        {
            Connection.Dispose();
        }
    }
}
