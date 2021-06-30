using System;

namespace DoomedDatabases.Postgres
{
    class DatabaseNameGenerator : IDatabaseNameGenerator
    {
        public string Prefix { get; set; } = "doomed_database_";

        public string Generate()
        {
            string id = Guid.NewGuid().ToString().Replace("-","");
            return $"{Prefix}{DateTime.UtcNow.Ticks}{id}";
        }
    }
}