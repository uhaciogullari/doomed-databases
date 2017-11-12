using Npgsql;

namespace DoomedDatabases.Postgres
{
    class ConnectionStringManager : IConnectionStringManager
    {
        public ConnectionStringManager(string defaultDatabaseConnectionString)
        {
            Default = defaultDatabaseConnectionString;
        }

        public void SetCreatedDatabaseName(string value)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(Default);
            connectionStringBuilder.Database = value;
            CreatedDatabase = connectionStringBuilder.ToString();
        }

        public string Default { get; private set; }
        public string CreatedDatabase { get; private set; }
    }
}