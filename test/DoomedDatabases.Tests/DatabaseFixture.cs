using System;
using DoomedDatabases.Postgres;
using DoomedDatabases.Tests.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DoomedDatabases.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            TestDatabase = new TestDatabaseBuilder().WithConfiguration(configuration).WithTemplateDatabase("").Build();
            TestDatabase.Create();

            var builder = new DbContextOptionsBuilder<TestDbContext>();
            builder.UseNpgsql(TestDatabase.ConnectionString);
            DbContext = new TestDbContext(builder.Options);
            DbContext.Database.EnsureCreated();
        }


        public ITestDatabase TestDatabase { get; }

        public TestDbContext DbContext { get; }

        public void Dispose()
        {
            TestDatabase.Drop();
        }
    }

    [CollectionDefinition("Database")]
    public class DatabaseCollectionFixture : ICollectionFixture<DatabaseFixture>
    {
    }
}