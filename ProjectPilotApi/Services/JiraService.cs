using Atlassian.Jira;
using Atlassian.Jira.Remote;
using Common;
using ProjectPilot.Exceptions;
using ProjectPilotWeb.Helpers;
using RestSharp;

namespace ProjectPilotWeb.Services;

public class JiraService
{
    protected readonly Jira Jira;
    private readonly string _projectKey;

    public JiraService(IConfiguration configuration)
    {
        var jiraServer = configuration["JiraConfig:BaseUrl"];
        var user = configuration["JiraConfig:User"];
        var password = configuration["JiraConfig:Password"];
        var projectKey = configuration["JiraConfig:ProjectKey"];
        
        if (jiraServer == null || user == null || password == null || projectKey == null)
        {
            throw new ConfigurationMissingException("Jira Config is missing or invalid.");
        }
        
        _projectKey = projectKey;
        var settings = new JiraRestClientSettings()
        {
            EnableRequestTrace = true,
        };
        settings.CustomFieldSerializers.Add("com.pyxis.greenhopper.jira:jsw-story-points", new CustomFieldSerializers.StoryPointSerializer());

        Jira = Jira.CreateRestClient(jiraServer, user, password, settings);
    }

    public async Task<IEnumerable<Issue>> GetIssuesAsync(string[] keys)
    {
        var issues = await Jira.Issues.GetIssuesAsync(keys);
        return issues.Values;
    }
    
    public IQueryable<Issue> GetIssuesAsQueryable()
    {
        return Jira.Issues.Queryable.Where(x => x.Project == _projectKey);
    }

    public async Task<Issue?> GetIssueAsync(string issueKey)
    {
        return await Jira.Issues.GetIssueAsync(issueKey);
    }

    public async Task UpdateIssueAsync(Issue issue)
    {
        await Jira.Issues.UpdateIssueAsync(issue);
    }

    public async Task<Project> GetProjectAsync()
    {
        return await Jira.Projects.GetProjectAsync(_projectKey);
    }

    public async Task UpdateIssueDescriptionAsync(string issueKey, string description, CancellationToken ct)
    {
        var issue = await Jira.Issues.GetIssueAsync(issueKey, ct);
        if (issue is null)
        {
            throw new WebApiException(StatusCodes.Status404NotFound, "Issue not found");
        }
        
        issue.Description = description;
        await Jira.Issues.UpdateIssueAsync(issue, ct);
    }
}