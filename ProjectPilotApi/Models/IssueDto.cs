namespace ProjectPilotWeb.Models;

public class IssueDto
{
    public string Key { get; set; }
    public string Type { get; set; }
    public string TypeIconUrl { get; set; }
    public string Summary { get; set; }
    public string Status { get; set; }
    public string? Description { get; set; }
    public string? ClientInput { get; set; }
    public string? Clarifications { get; set; }
    
    public int? StoryPoints { get; set; }
    public int? HoursEstimation { get; set; }
}