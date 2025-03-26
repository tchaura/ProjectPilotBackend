namespace ProjectPilotWeb.Models;

public class EstimationDto
{
    public int StoryPoints { get; set; }
    public int HoursEstimation { get; set; }
    public List<string> Justifications { get; set; }
    public string JustificationsMarkdown { get; set; }
}