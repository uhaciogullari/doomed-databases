namespace DoomedDatabases.Postgres
{
    interface IConnectionStringManager
    {
        void SetCreatedDatabaseName(string value);
        string Default { get; }
        string CreatedDatabase { get; }
    }
}