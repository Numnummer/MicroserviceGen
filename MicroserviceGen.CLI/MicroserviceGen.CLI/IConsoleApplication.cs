using System.Reflection;

namespace MicroserviceGen.CLI;

public interface IConsoleApplication
{
    void Run(string[] args);
    object? GetController(string flag);
    void InvokeHandler(object controller, string value);
    Action<object> CreateMethodDelegate(object controller, MethodInfo method);
}