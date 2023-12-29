using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DoomedDatabases.Tests.EntityFrameworkCore
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class TestDbContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UsePostgresConventions();
        }

        public DbSet<User> Users { get; set; }
    }

    static partial class ModelBuilderExtensions
    {
        public static void UsePostgresConventions(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName().ToSnakeCase());

                // Replace column names            
                foreach (IMutableProperty property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToSnakeCase());
                }

                foreach (IMutableKey key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                foreach (IMutableForeignKey key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                foreach (IMutableIndex index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
                }
            }

        }

        private static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = StartUnderscoreRegex().Match(input);
            return startUnderscores + SnakeCaseRegex().Replace(input, "$1_$2").ToLower();
        }

        [GeneratedRegex(@"([a-z0-9])([A-Z])")]
        private static partial Regex SnakeCaseRegex();

        [GeneratedRegex(@"^_+")]
        private static partial Regex StartUnderscoreRegex();
    }
}