using System.Text;

namespace MicroserviceGen.Domain;

public abstract class ScriptBuilderBase
{
    public abstract StringBuilder Script { get; }
    public abstract string Build();
    public abstract string WithEfCore();
}