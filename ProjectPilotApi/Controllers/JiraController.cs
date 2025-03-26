using Microsoft.AspNetCore.Mvc;
using ProjectPilotWeb.Helpers;
using ProjectPilotWeb.Models;
using ProjectPilotWeb.Services;

namespace ProjectPilotWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JiraController(JiraService service) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetIssues()
    {
        var issues = service
            .GetIssuesAsQueryable()
            .AsEnumerable()
            .ToList();
            var result = issues
            .Select((issue) => new IssueDto
        {
            Key = issue.Key.Value,
            TypeIconUrl = issue.Type.IconUrl,
            Type = issue.Type.Name,
            Summary = issue.Summary,
            Description = issue.Description,
            Clarifications = issue[JiraCustomFields.Clarifications]?.Value,
            Status = issue.Status.Name,
            ClientInput = issue[JiraCustomFields.ClientInput]?.Value,
            HoursEstimation = int.TryParse(issue[JiraCustomFields.HoursEstimation]?.Value, out var he) ? he : 0,
            StoryPoints = int.TryParse(issue[JiraCustomFields.StoryPoints]?.Value, out var sp) ? sp : 0,
        });
        
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{issueKey}")]
    public async Task<IActionResult> GetIssue(string issueKey, CancellationToken cancellationToken)
    {
        var issue = (await service.GetIssueAsync(issueKey));

        if (issue == null)
        {
            return NotFound();
        }
        
        var response = new IssueDto
        {
            Key = issue.Key.Value,
            Summary = issue.Summary,
            Description = issue.Description,
            // Clarifications = issue[JiraCustomFields.Clarifications].Value,
            Status = issue.Status.Name,
            ClientInput = issue[JiraCustomFields.ClientInput].Value,
            HoursEstimation = int.TryParse(issue[JiraCustomFields.HoursEstimation]?.Value, out var he) ? he : 0,
            StoryPoints = int.TryParse(issue[JiraCustomFields.HoursEstimation]?.Value, out var sp) ? sp : 0,
        };
        
        return Ok(response);
    }

    [HttpPut]
    [Route("{issueKey}")]
    public async Task<IActionResult> UpdateIssue(string issueKey, [FromBody] IssueDto issueDto,
        CancellationToken cancellationToken)
    {
        var issue = await service.GetIssueAsync(issueKey);
        if (issue is null)
        {
            NotFound("Issue not found");
        }
        
        issue.Summary = issueDto.Summary;
        issue.Description = issueDto.Description;
        issue[JiraCustomFields.ClientInput] = issueDto.ClientInput;
        issue[JiraCustomFields.Clarifications] = issueDto.Clarifications;
        issue[JiraCustomFields.HoursEstimation] = issueDto.HoursEstimation.ToString();
        issue[JiraCustomFields.StoryPoints] = issueDto.StoryPoints.ToString();
        
        await service.UpdateIssueAsync(issue);
        
        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateIssues([FromBody] List<IssueDto> issueDtos,
        CancellationToken cancellationToken)
    {
        if (issueDtos.Count == 0)
        {
            return NotFound("No issues to update");
        }
        
        var issues = (await service
                .GetIssuesAsync(issueDtos.Select(i => i.Key).ToArray()))
            .ToList();
        
        if (issues.Count != issueDtos.Count)
        {
            NotFound("Issues not found");
        }

        foreach (var issue in issues)
        {
            var issueDto = issueDtos.First(x => x.Key == issue.Key.Value);
            issue.Summary = issueDto.Summary;
            issue.Description = issueDto.Description;
            issue[JiraCustomFields.ClientInput] = issueDto.ClientInput;
            issue[JiraCustomFields.Clarifications] = issueDto.Clarifications;
            issue[JiraCustomFields.HoursEstimation] = issueDto.HoursEstimation.HasValue ? issueDto.HoursEstimation.Value.ToString() : "0";
            issue[JiraCustomFields.StoryPoints] = issueDto.StoryPoints.HasValue ? issueDto.StoryPoints.Value.ToString() : "0";
            await service.UpdateIssueAsync(issue);
        }
        
        return Ok("Updated successfully");
    }
}