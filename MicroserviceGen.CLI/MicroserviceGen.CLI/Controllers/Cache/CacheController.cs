using MicroserviceGen.CLI.Attributes;
using MicroserviceGen.CLI.Controllers.Cache.Scripts;
using MicroserviceGen.Domain;

namespace MicroserviceGen.CLI.Controllers.Cache;

[Controller("cache")]
public class CacheController
{
    [FlagHandler("redis")]
    public void HandleRedis()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.AddCommand(NLayerScripts.Redis); 
                break;
            case Architecture.Clean:
                break;
            case Architecture.Wqw:
                break;
            case Architecture.Mvc:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [FlagHandler("pgsql")]
    public void HandlePostgresCache()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.AddCommand(NLayerScripts.Postgres); 
                break;
            case Architecture.Clean:
                break;
            case Architecture.Wqw:
                break;
            case Architecture.Mvc:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}