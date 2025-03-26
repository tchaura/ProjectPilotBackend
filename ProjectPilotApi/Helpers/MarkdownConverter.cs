using System.Text;
using ProjectPilotWeb.Models;

namespace ProjectPilotWeb.Helpers;

public static class MarkdownConverter
{

    public static RequirementsDto ConvertToMarkdown(this ParsedRequirements parsedRequirements)
    {
        return new RequirementsDto()
        {
            Requirements = ConvertRequirementsToMarkdown(parsedRequirements),
            Clarifications = ConvertClarificationsToMarkdown(parsedRequirements)
        };

    }
    /// <summary>
    /// Converts the Requirements portion of a RequirementsResponse into a Markdown formatted string.
    /// </summary>
    private static string ConvertRequirementsToMarkdown(ParsedRequirements response)
    {
        var markdown = new StringBuilder();
        
        if (response?.Requirements != null)
        {
            var index = 1;
            foreach (var req in response.Requirements)
            {
                markdown.AppendLine($"{index++}. {req.Name}");

                if (req.Steps != null && req.Steps.Count > 0)
                {
                    foreach (var step in req.Steps)
                    {
                        markdown.AppendLine($"  - {step}");
                    }
                }
            }
        }

        return markdown.ToString();
    }

    /// <summary>
    /// Converts the Customer Clarifications portion of a RequirementsResponse into a Markdown formatted string.
    /// </summary>
    private static string ConvertClarificationsToMarkdown(ParsedRequirements response)
    {
        var markdown = new StringBuilder();

        if (response?.CustomerClarifications != null)
        {
            foreach (var clarification in response.CustomerClarifications)
            {
                markdown.AppendLine($"- {clarification}");
            }
        }

        return markdown.ToString();
    }

    public static EstimationDto ConvertToMarkdown(this EstimationDto estimationDto)
    {
        const string markDownListSymbol = "- ";
        estimationDto.JustificationsMarkdown = string.Join("\n", 
            estimationDto.Justifications.Select(x => markDownListSymbol + x));
        
        return estimationDto;
    }
}