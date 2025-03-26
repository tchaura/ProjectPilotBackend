using Atlassian.Jira;
using Common;
using ProjectPilot.Abstractions;
using ProjectPilot.Exceptions;
using ProjectPilotWeb.Helpers;
using ProjectPilotWeb.Models;

namespace ProjectPilotWeb.Services;

public class AiService(IAiClient aiClient, JiraService jiraService, ResponseFormatService responseFormatService, PromptService promptService)
{
    private string? _systemPrompt; 

    public async Task<ParsedRequirements> GetRequirementsByClientInputAsync(Issue issue, CancellationToken cancellationToken)
    {
        if (issue["Client input"] is null)
        {
            throw new ArgumentException("Client input is null");
        }
        
        const string requiredPromptId = PromptNames.GetRequirementsByClientInput;
        var responseFormat = responseFormatService.GetResponseFormatByName("RequirementsResponseFormat");
        var customPrompt = promptService.GetCustomPromptById(requiredPromptId);

        if (customPrompt is not { User: not null, System: not null })
        {
            throw new ConfigurationMissingException($"Missing required prompt {requiredPromptId}");
        }
        
        _systemPrompt ??= await promptService.GetInitialPromptWithProjectInfo();
        
        customPrompt.System = _systemPrompt + customPrompt.System;
        customPrompt.User = string.Format(customPrompt.User, issue.Summary, issue["Client input"], issue.Type.Name);
        
        string? output;
        ParsedRequirements result;
        
        // do
        // {
        //     output = await aiClient.SendTextMessage(enrichedPrompt, _systemPrompt, responseFormat, cancellationToken);
        //
        //     if (output is null)
        //     {
        //         throw new WebApiException(StatusCodes.Status503ServiceUnavailable, "AI Client error");
        //     }
        // } while (!output.TryParseJson(out result));
        
        output = await aiClient.SendTextMessage(customPrompt, responseFormat, cancellationToken);
        if (output is null)
        {
            throw new WebApiException(StatusCodes.Status503ServiceUnavailable, "AI Client error");
        }
        if (!output.TryParseJson(out result))
        {
            throw new WebApiException(StatusCodes.Status404NotFound, "Invalid AI response");
        }
        
        return result;
    }
    
    public async Task<ParsedRequirements> ImproveRequirementsAsync(Issue issue, CancellationToken cancellationToken)
    {
        if (issue["Client input"] is null)
        {
            throw new ArgumentException("Client input is null");
        }
        if (issue.Description is null)
        {
            throw new ArgumentException("No requirements provided");
        }
        
        var responseFormat = responseFormatService.GetResponseFormatByName(ResponseFormats.RequirementsResponseFormat);
        var customPrompt = promptService.GetCustomPromptById(PromptNames.ImproveRequirements);

        if (customPrompt is not { User: not null, System: not null })
        {
            throw new ConfigurationMissingException($"Missing required prompt {PromptNames.ImproveRequirements}");
        }
        
        _systemPrompt ??= await promptService.GetInitialPromptWithProjectInfo();
        
        customPrompt.System = _systemPrompt + customPrompt.System;
        customPrompt.User = string.Format(customPrompt.User, issue.Summary, issue[JiraCustomFields.ClientInput],
            issue.Description, issue[JiraCustomFields.Clarifications], issue.Type.Name);
        
        string? output;
        ParsedRequirements result;
        
        // do
        // {
        //     output = await aiClient.SendTextMessage(enrichedPrompt, _systemPrompt, responseFormat, cancellationToken);
        //
        //     if (output is null)
        //     {
        //         throw new WebApiException(StatusCodes.Status503ServiceUnavailable, "AI Client error");
        //     }
        // } while (!output.TryParseJson(out result));
        
        output = await aiClient.SendTextMessage(customPrompt, responseFormat, cancellationToken);
        if (output is null)
        {
            throw new WebApiException(StatusCodes.Status503ServiceUnavailable, "AI Client error");
        }
        if (!output.TryParseJson(out result))
        {
            throw new WebApiException(StatusCodes.Status404NotFound, "Invalid AI response");
        }
        
        return result;
    }
    
    public async Task<EstimationDto> EstimateTaskAsync(Issue issue, CancellationToken cancellationToken)
    {
        if (issue.Description is null)
        {
            throw new ArgumentException("No requirements provided");
        }
        
        var responseFormat = responseFormatService.GetResponseFormatByName(ResponseFormats.EstimationResponseFormat);
        var customPrompt = promptService.GetCustomPromptById(PromptNames.EstimateTask);

        if (customPrompt is not { User: not null, System: not null })
        {
            throw new ConfigurationMissingException($"Missing required prompt {PromptNames.EstimateTask}");
        }
        
        _systemPrompt ??= await promptService.GetInitialPromptWithProjectInfo();
        
        customPrompt.System = _systemPrompt + customPrompt.System;
        customPrompt.User = string.Format(customPrompt.User, issue.Summary, issue[JiraCustomFields.ClientInput],
            issue.Description, issue[JiraCustomFields.Clarifications], issue.Type.Name);
        
        string? output;
        EstimationDto result;
        
        // do
        // {
        //     output = await aiClient.SendTextMessage(enrichedPrompt, _systemPrompt, responseFormat, cancellationToken);
        //
        //     if (output is null)
        //     {
        //         throw new WebApiException(StatusCodes.Status503ServiceUnavailable, "AI Client error");
        //     }
        // } while (!output.TryParseJson(out result));
        
        output = await aiClient.SendTextMessage(customPrompt, responseFormat, cancellationToken);
        if (output is null)
        {
            throw new WebApiException(StatusCodes.Status503ServiceUnavailable, "AI Client error");
        }
        if (!output.TryParseJson(out result))
        {
            throw new WebApiException(StatusCodes.Status404NotFound, "Invalid AI response");
        }
        
        return result;
    }
    
    public async Task<EstimationDto> ReEstimateTaskAsync(Issue issue, CancellationToken cancellationToken)
    {
        if (issue.Description is null)
        {
            throw new ArgumentException("No requirements provided");
        }
        
        var responseFormat = responseFormatService.GetResponseFormatByName(ResponseFormats.EstimationResponseFormat);
        var customPrompt = promptService.GetCustomPromptById(PromptNames.EstimateTask);

        if (customPrompt is not { User: not null, System: not null })
        {
            throw new ConfigurationMissingException($"Missing required prompt {PromptNames.EstimateTask}");
        }
        
        _systemPrompt ??= await promptService.GetInitialPromptWithProjectInfo();
        
        customPrompt.System = _systemPrompt + customPrompt.System;
        customPrompt.User = string.Format(customPrompt.User, issue.Summary, issue[JiraCustomFields.ClientInput],
            issue.Description, issue[JiraCustomFields.Clarifications], issue[JiraCustomFields.StoryPoints],
            issue[JiraCustomFields.HoursEstimation], issue.Type.Name);
        
        string? output;
        EstimationDto result;
        
        // do
        // {
        //     output = await aiClient.SendTextMessage(enrichedPrompt, _systemPrompt, responseFormat, cancellationToken);
        //
        //     if (output is null)
        //     {
        //         throw new WebApiException(StatusCodes.Status503ServiceUnavailable, "AI Client error");
        //     }
        // } while (!output.TryParseJson(out result));
        
        output = await aiClient.SendTextMessage(customPrompt, responseFormat, cancellationToken);
        if (output is null)
        {
            throw new WebApiException(StatusCodes.Status503ServiceUnavailable, "AI Client error");
        }
        if (!output.TryParseJson(out result))
        {
            throw new WebApiException(StatusCodes.Status404NotFound, "Invalid AI response");
        }
        
        return result;
    }
}