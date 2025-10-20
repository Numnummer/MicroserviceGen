using MicroserviceGen.CLI.Attributes;

namespace MicroserviceGen.CLI.Controllers;

[Controller("logger")]
public class LoggerController
{
    [FlagHandler("nlog")]
    public void HandleNLog()
    {
        
    }

    [FlagHandler("serilog")]
    public void HandleSerilog()
    {
        
    }
}