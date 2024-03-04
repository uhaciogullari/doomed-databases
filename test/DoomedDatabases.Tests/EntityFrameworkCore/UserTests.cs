using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DoomedDatabases.Tests.EntityFrameworkCore
{
    [Collection("Database")]
    public class UserTests
    {
        private readonly TestDbContext testDbContext;

        public UserTests(DatabaseFixture databaseFixture)
        {
            testDbContext = databaseFixture.DbContext;
        }

        [Fact]
        public async Task InsertUsers()
        {
            await testDbContext.Users.AddAsync(new User { Username = "Pramod" });
            await testDbContext.Users.AddAsync(new User { Username = "Martin" });
            await testDbContext.SaveChangesAsync();
            
            var count = await testDbContext.Users.CountAsync();
            Assert.Equal(2, count);
        }
    }
}