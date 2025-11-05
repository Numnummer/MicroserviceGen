using System.Linq.Expressions;
using System.Reflection;
using MicroserviceGen.CLI.Attributes;
using MicroserviceGen.CLI.Controllers;
using MicroserviceGen.CLI.Controllers.Api;

namespace MicroserviceGen.CLI;

public class Application:IConsoleApplication
{
    public async Task RunAsync(string[] args)
    {
        var flags = args
            .Select(arg => arg.Split('='))
            .ToDictionary(parts => parts[0].TrimStart('-'), parts => parts.Length > 1 ? parts[1] : string.Empty);

        // Флаг --template обрабатываем отдельно в первую очередь,
        // так как это задает базу скрипта.
        if (flags.TryGetValue("template", out var template))
        {
            var templateController = new BaseScriptController();
            await templateController.InitBaseScriptAsync(template);
        }

        if (!flags.ContainsKey("api"))
        {
            // Если не задан api, то по умолчанию web.
            var controller = new ApiController();
            controller.HandleWeb();
        }
        
        foreach (var flag in flags)
        {
            // Флаги обрабатываем, находя контроллеры по атрибутам,
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

        await Script.Instance.RunScriptAsync();
        //Console.WriteLine(Script.Instance.ScriptText);
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