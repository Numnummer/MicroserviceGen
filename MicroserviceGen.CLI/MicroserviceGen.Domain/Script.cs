using System.Diagnostics;
using System.Text;

namespace MicroserviceGen.Domain;

public sealed class Script
{
    private static readonly Lazy<Script> lazy = new(() => new Script());
    public static Script Instance => lazy.Value;

    private StringBuilder ScriptText { get; set; }
    public Architecture Architecture { get; private set; }

    public async Task RunScriptAsync()
    {
        var tempScriptPath = Path.GetTempFileName();
    
        try
        {
            // Сохраняем скрипт во временный файл
            await File.WriteAllTextAsync(tempScriptPath, ScriptText.ToString());
        
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
        return OperatingSystem.IsWindows() ? "cmd.exe" : "/bin/bash";
    }

    private string GetShellArguments(string scriptPath)
    {
        return OperatingSystem.IsWindows() ? $"/c \"{scriptPath}\"" : $"\"{scriptPath}\"";
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
        ScriptText.Append(command);
    }
    
    /// <summary>
    /// Вставить команду после первой найденной подстроки after.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="after"></param>
    public void AddCommandAfter(string command, string after)
    {
        var parts = ScriptText.ToString().Split(after);
        string[] newParts = [parts[0], after, command, ..parts[1..]];
        ScriptText.Clear().Append(string.Join("\n", newParts));
    }

    /// <summary>
    /// Вставить команду между regionStart и regionEnd, заменяя
    /// при этом содержимое между ними.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="regionStart"></param>
    /// <param name="regionEnd"></param>
    public void PlaceCommandInRegion(string command, string regionStart, string regionEnd)
    {
        var scriptString = ScriptText.ToString();
        var startPosition = scriptString.IndexOf(regionStart) + regionStart.Length;
        var endPosition = scriptString.IndexOf(regionEnd);
        var parts = scriptString.Split(scriptString[startPosition..endPosition]);
        string[] newParts = [parts[0], command, parts[1]];
        ScriptText.Clear().Append(string.Join("\n", newParts));
    }

    public void ReplaceAll(string oldValue, string newValue)
    {
        ScriptText.Replace(oldValue, newValue);
    }

    public bool TryReplaceRow(string contents, string newRow)
    {
        var script = ScriptText.ToString();
        var rows = script.Split('\n');
        var row = rows.FirstOrDefault(r => r.Contains(contents));
        if (row != null)
        {
            ScriptText.Replace(row, newRow);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Используется для получения регионов из скрипта.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns>Текст скрипта между параметрами, включая их</returns>
    public string? GetTextBetween(string start, string end)
    {
        var startIndex = ScriptText.ToString().IndexOf(start, StringComparison.Ordinal);
        if (startIndex == -1) return null;
        var endIndex = ScriptText.ToString().IndexOf(end, StringComparison.Ordinal);
        if (endIndex == -1) return null;
        return ScriptText.ToString().Substring(startIndex+start.Length, endIndex-start.Length-startIndex);
    }

    public void Initialize(string baseScript, Architecture architecture)
    {
        Architecture = architecture;
        ScriptText.Append(baseScript);
    }

    private Script()
    {
        ScriptText = new StringBuilder();
    }
}