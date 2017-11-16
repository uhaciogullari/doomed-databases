using System;
using Microsoft.Extensions.Configuration;

namespace DoomedDatabases.Postgres
{
    public class TestDatabaseBuilder
    {
        private string connectionString;
        private string connectionStringName = "DefaultConnection";
        private string templateDatabase;
        private IConfiguration configuration;
        private readonly DatabaseNameGenerator databaseNameGenerator = new DatabaseNameGenerator();

        public TestDatabaseBuilder WithConnectionString(string value)
        {
            connectionString = value;
            return this;
        }

        public TestDatabaseBuilder WithConfiguration(IConfiguration value)
        {
            configuration = value;
            return this;
        }

        public TestDatabaseBuilder WithConnectionStringName(string value)
        {
            connectionStringName = value;
            return this;
        }

        public TestDatabaseBuilder WithDatabaseNamePrefix(string value)
        {
            databaseNameGenerator.Prefix = value;
            return this;
        }

        public TestDatabaseBuilder WithTemplateDatabase(string value)
        {
            templateDatabase = value;
            return this;
        }

        public ITestDatabase Build()
        {
            if (string.IsNullOrWhiteSpace(connectionString) && configuration == null)
            {
                throw new ArgumentException("No connection string or configuration was provided");
            }

            if (configuration != null)
            {
                connectionString = configuration.GetConnectionString(connectionStringName);
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new ArgumentException($"Could not find connection string with name: {connectionStringName}");
                }
            }

            return new TestDatabase(new ConnectionStringManager(connectionString), databaseNameGenerator, new Connection())
            {
                TemplateDatabase = templateDatabase
            };
        }
    }
}