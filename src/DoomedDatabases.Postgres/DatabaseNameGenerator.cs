using System;

namespace DoomedDatabases.Postgres
{
    class DatabaseNameGenerator : IDatabaseNameGenerator
    {
        public string Prefix { get; set; } = "doomed_database_";

        public string Generate()
        {
            return $"{Prefix}{DateTime.UtcNow.Ticks}";
        }
    }
}