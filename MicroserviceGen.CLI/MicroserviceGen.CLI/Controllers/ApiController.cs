using MicroserviceGen.CLI.Attributes;

namespace MicroserviceGen.CLI.Controllers;

[Controller("api")]
public class ApiController
{
    [FlagHandler("graphql")]
    public void HandleGraphQL()
    {
        
    }

    [FlagHandler("grpc")]
    public void HandleGrpc()
    {
        
    }

    [FlagHandler("web")]
    public void HandleWeb()
    {
        
    }

    [FlagHandler("swagger")]
    public void HandleSwagger()
    {
        
    }
}