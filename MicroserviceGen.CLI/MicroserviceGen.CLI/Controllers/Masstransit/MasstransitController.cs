using MicroserviceGen.CLI.Attributes;
using MicroserviceGen.CLI.Controllers.Masstransit.Scripts;
using MicroserviceGen.Domain;

namespace MicroserviceGen.CLI.Controllers.Masstransit;

[Controller("masstransit")]
public class MasstransitController
{
    [FlagHandler("kafka")]
    public void HandleKafka()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.AddCommand(NLayerScripts.Kafka); 
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
    
    [FlagHandler("rabbit")]
    public void HandleRabbitMq()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.AddCommand(NLayerScripts.Rabbit); 
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