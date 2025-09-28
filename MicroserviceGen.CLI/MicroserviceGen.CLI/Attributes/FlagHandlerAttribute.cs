namespace MicroserviceGen.CLI.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class FlagHandlerAttribute(string flag) : Attribute
{
    public string Flag { get; } = flag;
}
