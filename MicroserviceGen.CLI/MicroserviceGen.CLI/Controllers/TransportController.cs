using MicroserviceGen.CLI.Attributes;

namespace MicroserviceGen.CLI.Controllers;

[Controller("masstransit")]
public class MasstransitController
{
    [FlagHandler("kafka")]
    public void HandleKafka()
    {
        Console.WriteLine("Configured to use Kafka for transport.");
    }
    
    [FlagHandler("rabbit")]
    public void HandleRabbitMq()
    {
        Console.WriteLine("Configured to use RabbitMQ for transport.");
    }
}