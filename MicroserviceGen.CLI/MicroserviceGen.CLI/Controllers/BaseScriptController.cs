using MicroserviceGen.Domain;

namespace MicroserviceGen.CLI.Controllers;

public class BaseScriptController
{
    /// <summary>
    /// По пути к шаблону, создает скрипт создания микросервиса.
    /// Предполагается, что в первой строке скрипта будет указана
    /// архитектура в виде #arch_[название как в enum Architexture].
    /// </summary>
    /// <param name="baseScriptPath">Путь к шаблону</param>
    public async Task InitBaseScriptAsync(string baseScriptPath)
    {
        var lines=new List<string>();
        await foreach (var line in File.ReadLinesAsync(baseScriptPath))
        {
            lines.Add(line);
        }
        var content = string.Join('\n', lines.Skip(1));
        var arch = lines.First()[6..];
        if (Enum.TryParse(arch, out Architecture architecture))
        {
            Script.Instance.Initialize(content, architecture);
        }
        else
        {
            Console.WriteLine($"Не удалось определить архитектуру: {lines.First()}");
        }
    }
}