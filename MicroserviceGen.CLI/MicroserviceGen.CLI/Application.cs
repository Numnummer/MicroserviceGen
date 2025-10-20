using System.Linq.Expressions;
using System.Reflection;
using MicroserviceGen.CLI.Attributes;
using MicroserviceGen.CLI.Controllers;

namespace MicroserviceGen.CLI;

public class Application:IConsoleApplication
{
    public void Run(string[] args)
    {
        var flags = args
            .Select(arg => arg.Split('='))
            .ToDictionary(parts => parts[0].TrimStart('-'), parts => parts.Length > 1 ? parts[1] : string.Empty);

        foreach (var flag in flags)
        {
            // Флаг --template отследим отдельно для простоты.
            if (flag.Key == "template")
            {
                var templateController = new BaseScriptController();
                templateController.InitBaseScript(flag.Value);
                continue;
            }
            
            // Остальные флаги обрабатываем, находя контроллеры по атрибутам,
            // чтобы код и архитектура решения были более читаемы.
            var controller = GetController(flag.Key);
            if (controller != null)
            {
                InvokeHandler(controller, flag.Value);
            }
            else
            {
                Console.WriteLine($"No controller found for flag '{flag.Key}'.");
            }
        }
    }

    public object? GetController(string flag)
    {
        var controllerType = Assembly.GetExecutingAssembly().GetTypes()
            .FirstOrDefault(t => t.GetCustomAttributes(typeof(ControllerAttribute), false)
                .Cast<ControllerAttribute>().Any(attr => attr.Name == flag));

        return controllerType != null ? Activator.CreateInstance(controllerType) : null;
    }

    public void InvokeHandler(object controller, string value)
    {
        var methods = controller.GetType()
            .GetMethods()
            .Where(m => m.GetCustomAttributes(typeof(FlagHandlerAttribute), false)
                .Cast<FlagHandlerAttribute>()
                .Any(attr => attr.Flag == value));

        var methodInfos = methods as MethodInfo[] ?? methods.ToArray();
        if (methodInfos.Any())
        {
            foreach (var method in methodInfos)
            {
                var action = CreateMethodDelegate(controller, method);
                action.Invoke(controller);
            }
        }
        else
        {
            Console.WriteLine($"No handler found for value '{value}'.");
        }
    }

    public Action<object> CreateMethodDelegate(object controller, MethodInfo method)
    {
        var controllerParameter = Expression.Parameter(typeof(object), "controller");
        var instance = Expression.Convert(controllerParameter, controller.GetType());
        var methodCall = Expression.Call(instance, method);

        var lambda = Expression.Lambda<Action<object>>(methodCall, controllerParameter);
        return lambda.Compile();
    }
}