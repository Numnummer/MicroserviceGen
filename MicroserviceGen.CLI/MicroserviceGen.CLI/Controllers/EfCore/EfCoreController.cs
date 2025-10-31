using MicroserviceGen.CLI.Attributes;
using MicroserviceGen.CLI.Controllers.EfCore.Scripts;
using MicroserviceGen.Domain;

namespace MicroserviceGen.CLI.Controllers.EfCore;

[Controller("efcore")]
public class EfCoreController
{
    [FlagHandler("psql")]
    public void HandlePostgres()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.AddCommand(NLayerScripts.Pgsql); 
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

    [FlagHandler("sqlserv")]
    public void HandleSqlserv()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.AddCommand(NLayerScripts.Sqlserv); 
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

    [FlagHandler("sqlite")]
    public void HandleSqlite()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.AddCommand(NLayerScripts.Sqlite); 
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