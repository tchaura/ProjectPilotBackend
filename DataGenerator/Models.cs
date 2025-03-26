using System.Text.Json.Serialization;

namespace DataGenerator;

public class JiraTask
{
    [JsonPropertyName("fields")]
    public required Fields Fields { get; set; }
}

public class Fields
{
    [JsonPropertyName("project")]
    public required Project Project { get; set; }

    [JsonPropertyName("issuetype")]
    public required IssueType IssueType { get; set; }

    [JsonPropertyName("summary")]
    public required string Summary { get; set; }

    [JsonPropertyName("client_input")]
    public string? ClientInput { get; set; }
}

public class Project
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }
}

public class IssueType
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}