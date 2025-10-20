namespace MicroserviceGen.CLI;

public sealed class Script
{
    private static readonly Lazy<Script> lazy = new(() => new Script());
    public static Script Instance => lazy.Value;
    
    public string ScriptText { get;  }

    public void RunScript()
    {
        
    }

    public void AddCommand(string command)
    {
        
    }

    private Script()
    {
        
    }
}