using System.Text.Json;
using Atlassian.Jira;
using Microsoft.Extensions.Configuration;
using ProjectPilotWeb.Services;

namespace DataGenerator;

public class TasksGenerator: JiraService
{
    private readonly string _jsonPath;
    
    public TasksGenerator(IConfiguration configuration, string jsonPath): base(configuration)
    {
        _jsonPath = jsonPath;
    }
    // Method to load tasks from JSON
    private List<JiraTask> LoadTasksFromJson()
    {
        try
        {
            // Read the JSON file
            var json = File.ReadAllText(_jsonPath);

            // Deserialize JSON to List<Dictionary<string, object>>
            var tasks = JsonSerializer.Deserialize<List<JiraTask>>(json);

            if (tasks != null)
            {
                Console.WriteLine("Tasks successfully loaded from JSON.");
                return tasks;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading tasks from JSON: {ex.Message}");
        }

        return new List<JiraTask>();
    }
    
    public async Task AddTasksToJira()
    {
        var tasks = LoadTasksFromJson();
        foreach (var task in tasks)
        {
            try
            {
                var fields = task.Fields;
                var projectKey = fields.Project.Key;
                var summary = fields.Summary;
                var issueType = fields.IssueType.Name;
                var clientInput = fields.ClientInput;

                var issueExists = Jira.Issues.Queryable.Any(x => x.Summary.Equals(summary));
                if (issueExists)
                {
                    Console.WriteLine($"Task {summary} already exists. Skipping.");
                    continue;
                }
                var issue = Jira.CreateIssue(projectKey);
                issue.Type = issueType;
                issue.Summary = summary;
                issue["Client input"] = clientInput;

                await issue.SaveChangesAsync();
                Console.WriteLine($"Задача '{summary}' успешно добавлена в проект {projectKey}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении задачи: {ex.Message}");
            }
        }

    }
}