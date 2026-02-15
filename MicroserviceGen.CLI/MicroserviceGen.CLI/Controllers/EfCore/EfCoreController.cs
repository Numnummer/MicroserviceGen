using MicroserviceGen.CLI.Attributes;
using MicroserviceGen.CLI.Controllers.EfCore.Scripts;
using MicroserviceGen.Domain;

namespace MicroserviceGen.CLI.Controllers.EfCore;

[Controller("efcore")]
public class EfCoreController
{
    const string startEfCoreRegion = "#efcore";
    const string endEfCoreRegion = "#endefcore";
    [FlagHandler("psql")]
    public void HandlePostgres()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.TryReplaceTriggerCommandsFromAnotherScriptInRegion(startEfCoreRegion, endEfCoreRegion,
                 NLayerScripts.Pgsql, "#specific_provider"); 
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
                Script.Instance.TryReplaceTriggerCommandsFromAnotherScriptInRegion(startEfCoreRegion, endEfCoreRegion,
                 NLayerScripts.Sqlserv, "#specific_provider"); 
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
                Script.Instance.TryReplaceTriggerCommandsFromAnotherScriptInRegion(startEfCoreRegion, endEfCoreRegion,
                 NLayerScripts.Sqlite, "#specific_provider"); 
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