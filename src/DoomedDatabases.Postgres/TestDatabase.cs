using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DoomedDatabases.Postgres
{
    class TestDatabase : ITestDatabase
    {
        private readonly IConnectionStringManager connectionStringManager;
        private readonly IDatabaseNameGenerator databaseNameGenerator;
        private readonly IConnection connection;
        private string createdDatabaseName;

        public TestDatabase(IConnectionStringManager connectionStringManager, IDatabaseNameGenerator databaseNameGenerator, IConnection connection)
        {
            this.connectionStringManager = connectionStringManager;
            this.databaseNameGenerator = databaseNameGenerator;
            this.connection = connection;
        }

        public void Create()
        {
            createdDatabaseName = databaseNameGenerator.Generate();
            connectionStringManager.SetCreatedDatabaseName(createdDatabaseName);
            ConnectionString = connectionStringManager.CreatedDatabase;
            connection.Execute(connectionStringManager.Default, BuildCreateStatement());
        }

        private string BuildCreateStatement()
        {
            StringBuilder stringBuilder = new StringBuilder($"create database {createdDatabaseName}");

            if (!string.IsNullOrWhiteSpace(DatabaseTemplate))
            {
                stringBuilder.Append($" template {DatabaseTemplate}");
            }

            stringBuilder.Append(";");

            return stringBuilder.ToString();
        }

        public void RunScripts(string scriptFolderPath)
        {
            if (!Directory.Exists(scriptFolderPath))
            {
                throw new ArgumentException($"Directory does not exist: {scriptFolderPath}");
            }

            Directory.GetFiles(scriptFolderPath)
                     .OrderBy(s => s)
                     .Select(File.ReadAllText)
                     .ToList()
                     .ForEach(command => connection.Execute(connectionStringManager.CreatedDatabase, command));
        }

        public string ConnectionString { get; private set; }

        public void Drop()
        {
            connection.Execute(connectionStringManager.Default, $"select pid, pg_terminate_backend(pid) FROM pg_stat_activity where datname = '{createdDatabaseName}' AND pid <> pg_backend_pid();");
            connection.Execute(connectionStringManager.Default, $"drop database {createdDatabaseName};");
        }

        public string DatabaseTemplate { get; set; }
    }
}