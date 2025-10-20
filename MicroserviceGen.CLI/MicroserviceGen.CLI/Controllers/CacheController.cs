using MicroserviceGen.CLI.Attributes;

namespace MicroserviceGen.CLI.Controllers;

[Controller("cache")]
public class CacheController
{
    [FlagHandler("redis")]
    public void HandleRedis()
    {
        Console.WriteLine("Configured to use Redis cache.");
    }

    [FlagHandler("pgsql")]
    public void HandleMemoryCache()
    {
        Console.WriteLine("Configured to use in-memory cache.");
    }
}