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
                var regionStart = "#api_begin";
                var regionEnd = "#api_end";
                var apiScript = Script.Instance.GetTextBetween(regionStart, regionEnd);
                var script = NLayerScripts.Web + NLayerScripts.Graphql;
                if (apiScript != null)
                {
                    Script.Instance.PlaceCommandInRegion(script, regionStart, regionEnd);
                    return;
                }
                script = regionStart + '\n' + script + '\n' + regionEnd;
                Script.Instance.AddCommand(script); 
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
                var regionStart = "#api_begin";
                var regionEnd = "#api_end";
                Script.Instance.PlaceCommandInRegion(NLayerScripts.Grpc, regionStart, regionEnd); 
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
                var regionStart = "#api_begin";
                var regionEnd = "#api_end";
                Script.Instance.PlaceCommandInRegion(NLayerScripts.Web, regionStart, regionEnd); 
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