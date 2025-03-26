using System.ClientModel;
using Common.Models;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using ProjectPilot.Abstractions;
using ProjectPilot.Exceptions;
using Together;
using Together.AI;
using HttpClient = System.Net.Http.HttpClient;

namespace ProjectPilot;

public class TogetherXyzAiClient: IAiClient
{
    private readonly TogetherAIClient _chatClient;
    
    public TogetherXyzAiClient(IConfiguration configuration)
    {
        if (!configuration.GetSection("AiConfig").Exists())
        {
            throw new ConfigurationMissingException("AI configuration is missing");
        }
        
        var apiKey = configuration.GetSection("AiConfig:ApiKey").Value!;

        var customClient = new HttpClient();
        customClient.SetupClient(apiKey: apiKey);
        _chatClient = new TogetherAIClient(customClient);
    }

    public async Task<string?> SendTextMessage(
        CustomPrompt prompt,
        Dictionary<string, object>? responseFormat = null, 
        CancellationToken ct = default)
    {
        var messages = new List<TogetherAIChatMessage>();

        if (!string.IsNullOrEmpty(prompt.System))
        {
            messages.Add(new TogetherAIChatSystemMessage(prompt.System));
        }
        
        if (!string.IsNullOrEmpty(prompt.User))
        {
            messages.Add(new TogetherAIChatUserMessage(prompt.User));
        }
        
        _args.Messages = messages;
        _args.ResponseFormat = new TogetherAIResponseFormat
        {
            Schema = responseFormat,
            Type = "json_object"
        };
        
        var result = await _chatClient.GetChatCompletionsAsync(_args, ct);
        return result!.Choices![0].Message!.Content!;
    }

    private readonly TogetherAIChatCompletionArgs _args = new()
    {
        Model = TogetherAiModels.MetaLlama,
    };
}