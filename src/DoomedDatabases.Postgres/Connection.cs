using Npgsql;

namespace DoomedDatabases.Postgres
{
    class Connection : IConnection
    {
        public void Execute(string connectionString, string command)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var npgsqlCommand = connection.CreateCommand())
                {
                    npgsqlCommand.CommandText = command;
                    npgsqlCommand.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}