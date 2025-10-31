using MicroserviceGen.CLI.Attributes;
using MicroserviceGen.CLI.Controllers.Api.Scripts;
using MicroserviceGen.Domain;

namespace MicroserviceGen.CLI.Controllers.Api;

[Controller("api")]
public class ApiController
{
    [FlagHandler("graphql")]
    public void HandleGraphQL()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.AddCommand(NLayerScripts.Graphql); 
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

    [FlagHandler("grpc")]
    public void HandleGrpc()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                var after = "dotnet new sln --name _PasteNameHere_";
                Script.Instance.AddCommandAfter(NLayerScripts.Grpc, after); 
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

    [FlagHandler("web")]
    public void HandleWeb()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                var after = "dotnet new sln --name _PasteNameHere_";
                Script.Instance.AddCommandAfter(NLayerScripts.Web, after); 
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

    [FlagHandler("swagger")]
    public void HandleSwagger()
    {
        var arch = Script.Instance.Architecture;
        switch (arch)
        {
            case Architecture.NLayer:
                Script.Instance.AddCommand(NLayerScripts.Swagger); 
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