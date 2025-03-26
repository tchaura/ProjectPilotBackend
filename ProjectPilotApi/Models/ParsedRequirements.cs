namespace ProjectPilotWeb.Models;

public class ParsedRequirements
{
    public List<Requirement> Requirements { get; set; }
    public List<string> CustomerClarifications { get; set; }
}

public class Requirement
{
    public string Name { get; set; }
    public List<string> Steps { get; set; }
}