using System.Diagnostics;
using MicroserviceGen.Domain;

namespace MicroserviceGen.CLI;

public sealed class Script
{
    private static readonly Lazy<Script> lazy = new(() => new Script());
    public static Script Instance => lazy.Value;
    
    public string ScriptText { get; private set; }
    public Architecture Architecture { get; private set; }

    public async Task RunScriptAsync()
    {
        var tempScriptPath = Path.GetTempFileName();
    
        try
        {
            // Сохраняем скрипт во временный файл
            await File.WriteAllTextAsync(tempScriptPath, ScriptText);
        
            // Делаем файл исполняемым (для Unix-систем)
            if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                Chmod(tempScriptPath, "755");
            }

            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = GetShellExecutable(),
                Arguments = GetShellArguments(tempScriptPath),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();

            // Асинхронное чтение вывода
            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            var output = await outputTask;
            var error = await errorTask;

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"Script execution failed: {error}");
            }

            Console.WriteLine($"Output: {output}");
        }
        finally
        {
            // Удаляем временный файл
            if (File.Exists(tempScriptPath))
            {
                File.Delete(tempScriptPath);
            }
        }
    }

    private string GetShellExecutable()
    {
        if (OperatingSystem.IsWindows())
            return "cmd.exe";
        else
            return "/bin/bash";
    }

    private string GetShellArguments(string scriptPath)
    {
        if (OperatingSystem.IsWindows())
            return $"/c \"{scriptPath}\"";
        else
            return $"\"{scriptPath}\"";
    }

    private void Chmod(string filePath, string permissions)
    {
        if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
        {
            Process.Start("chmod", $"{permissions} \"{filePath}\"")?.WaitForExit();
        }
    }

    public void AddCommand(string command)
    {
        ScriptText += command;
    }
    
    /// <summary>
    /// Вставить команду после первой найденной подстроки after.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="after"></param>
    public void AddCommandAfter(string command, string after)
    {
        var parts = ScriptText.Split(after);
        string[] newParts = [parts[0], after, command, ..parts[1..]];
        ScriptText = string.Join("\n", newParts);
    }

    public void Initialize(string baseScript, Architecture architecture)
    {
        Architecture = architecture;
        ScriptText = baseScript;
    }

    private Script()
    {
        ScriptText = string.Empty;
    }
}