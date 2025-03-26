namespace Common.Models;

public class Prompts
{
    public string InitialPrompt { get; set; }
    public Dictionary<string, CustomPrompt> CustomPrompts { get; set; }
}

public class CustomPrompt
{
    public string? System { get; set; }
    public string? User { get; set; }
}

