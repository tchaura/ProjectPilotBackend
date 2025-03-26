using Common.Models;
using Microsoft.AspNetCore.Mvc;
using ProjectPilot.Abstractions;
using ProjectPilotWeb.Helpers;
using ProjectPilotWeb.Models;
using ProjectPilotWeb.Services;

namespace ProjectPilotWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiClientController(IAiClient aiClient, AiService aiService, JiraService jiraService) : Controller
{
    // GET
    [HttpPost]
    [Route("GetResponse")]
    public async Task<IActionResult> Index([FromBody] string query)
    {
        var prompt = new CustomPrompt { User = query };
        var response = await aiClient.SendTextMessage(prompt);
        return Ok(response);
    }

    [HttpGet]
    [Route("GetRequirementsByClientInput/{issueKey:required}")]
    public async Task<IActionResult> GetRequirementsByClientInput(string issueKey,
        CancellationToken ct)
    {
        var issue = await jiraService.GetIssueAsync(issueKey);
        if (issue is null)
        {
            return NotFound("Issue not found");
        }
        
        var result = await aiService.GetRequirementsByClientInputAsync(issue, ct);
        return Ok(result.ConvertToMarkdown());
    }

    [HttpGet]
    [Route("ImproveRequirements/{issueKey:required}")]
    public async Task<IActionResult> GetImproveRequirements(string issueKey, CancellationToken ct)
    {
        var issue = await jiraService.GetIssueAsync(issueKey);
        if (issue is null)
        {
            return NotFound("Issue not found");
        }
        
        var result = await aiService.ImproveRequirementsAsync(issue, ct);
        return Ok(result.ConvertToMarkdown());
    }
    
    [HttpGet]
    [Route("EstimateTask/{issueKey:required}")]
    public async Task<IActionResult> EstimateTask(string issueKey, CancellationToken ct)
    {
        var issue = await jiraService.GetIssueAsync(issueKey);
        if (issue is null)
        {
            return NotFound("Issue not found");
        }
        
        var result = await aiService.EstimateTaskAsync(issue, ct);
        return Ok(result.ConvertToMarkdown());
    }
    
    [HttpGet]
    [Route("ReEstimateTask/{issueKey:required}")]
    public async Task<IActionResult> ReEstimateTask(string issueKey, CancellationToken ct)
    {
        var issue = await jiraService.GetIssueAsync(issueKey);
        if (issue is null)
        {
            return NotFound("Issue not found");
        }
        
        var result = await aiService.ReEstimateTaskAsync(issue, ct);
        return Ok(result.ConvertToMarkdown());
    }
}