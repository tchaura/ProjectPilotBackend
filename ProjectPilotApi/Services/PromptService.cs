using Common.Models;
using Newtonsoft.Json;
using ProjectPilot.Exceptions;

namespace ProjectPilotWeb.Services;

public class PromptService(JiraService jiraService)
{
    private readonly Prompts _prompts = GetPromptsAsync().GetAwaiter().GetResult();

    
    public async Task<string> GetInitialPromptWithProjectInfo()
    {
        var prompt = _prompts.InitialPrompt;
        var file = await File.ReadAllTextAsync("../Common/ProjectInfo/projectInfo.json");
        var projectInfo = JsonConvert.DeserializeObject<ProjectInfo>(file);

        if (prompt is null)
        {
            throw new ConfigurationMissingException("System prompt is missing.");
        }
        if (projectInfo is null)
        {
            throw new ConfigurationMissingException("Project info is missing.");
        }
        
        var project = await jiraService.GetProjectAsync();
        
        return string.Format(prompt, project.Name, projectInfo.Description, projectInfo.TechStack);
    }
    
    public CustomPrompt? GetCustomPromptById(string id)
    {
        return _prompts?.CustomPrompts.GetValueOrDefault(id);
    }
    
    private static async Task<Prompts> GetPromptsAsync()
    {
        var text = await File.ReadAllTextAsync( "../Common/Prompts/prompts.json");
        return JsonConvert.DeserializeObject<Prompts>(text)!;
    }
}