using System;
using DoomedDatabases.Postgres;
using Xunit;

namespace DoomedDatabases.Tests
{
    public class DatabaseFixture : IDisposable
    {
        private readonly ITestDatabase testDatabase;

        public DatabaseFixture()
        {
            testDatabase = new TestDatabaseBuilder().WithConnectionString("User ID=integration_test_user;Password=432423ff;Server=localhost;Database=postgres;").Build();
            testDatabase.Create();
        }

        public void Dispose()
        {
            testDatabase.Drop();
        }
    }

    [CollectionDefinition("Database")]
    public class DatabaseCollectionFixture : ICollectionFixture<DatabaseFixture>
    {
    }
}