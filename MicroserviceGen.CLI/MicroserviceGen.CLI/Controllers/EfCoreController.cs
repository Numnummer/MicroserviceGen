using MicroserviceGen.CLI.Attributes;

namespace MicroserviceGen.CLI;

[Controller("efcore")]
public class EfCoreController
{
    [FlagHandler("psql")]
    public void HandlePostgres()
    {
        Console.WriteLine("Configured to use PostgreSQL with EF Core.");
    }

    [FlagHandler("sqlserv")]
    public void HandleMySql()
    {
        Console.WriteLine("Configured to use MySQL with EF Core.");
    }

    [FlagHandler("sqlite")]
    public void HandleSqlite()
    {
        Console.WriteLine("Configured to use SQLite with EF Core.");
    }
}