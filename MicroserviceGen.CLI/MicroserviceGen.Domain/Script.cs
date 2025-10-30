using MicroserviceGen.Domain;

namespace MicroserviceGen.CLI;

public sealed class Script
{
    private static readonly Lazy<Script> lazy = new(() => new Script());
    public static Script Instance => lazy.Value;
    
    public string ScriptText { get; private set; }
    public Architecture Architecture { get; private set; }

    public void RunScript()
    {
        
    }

    public void AddCommand(string command)
    {
        ScriptText += command;
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