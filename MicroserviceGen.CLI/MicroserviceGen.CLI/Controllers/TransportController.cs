using MicroserviceGen.CLI.Attributes;

namespace MicroserviceGen.CLI.Controllers;

[Controller("transport")]
public class TransportController
{
    [FlagHandler("kafka")]
    public void HandleKafka()
    {
        Console.WriteLine("Configured to use Kafka for transport.");
    }
    
    [FlagHandler("rabbitmq")]
    public void HandleRabbitMQ()
    {
        Console.WriteLine("Configured to use RabbitMQ for transport.");
    }
}