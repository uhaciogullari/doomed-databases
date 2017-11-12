namespace DoomedDatabases.Postgres
{
    interface IConnection
    {
        void Execute(string connectionString, string command);
    }
}