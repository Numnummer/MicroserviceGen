using MicroserviceGen.CLI.Attributes;
using MicroserviceGen.CLI.Controllers.Logger.Scripts;
using MicroserviceGen.Domain;

namespace MicroserviceGen.CLI.Controllers.Logger;

[Controller("logger")]
public class LoggerController
{
    [FlagHandler("nlog")]
    public void HandleNLog()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.AddCommand(NLayerScripts.NLog); 
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

    [FlagHandler("serilog")]
    public void HandleSerilog()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.AddCommand(NLayerScripts.Serilog); 
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