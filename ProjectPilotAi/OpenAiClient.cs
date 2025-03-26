using System.ClientModel;
using Common.Models;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using ProjectPilot.Abstractions;
using ProjectPilot.Exceptions;
using Together;
using Together.AI;
using ChatClient = OpenAI.Chat.ChatClient;

namespace ProjectPilot;

public class OpenAiClient: IAiClient
{
    private readonly ChatClient _chatClient;

    private const string CurrentModel = TogetherAiModels.MetaLlama;
    
    public OpenAiClient(IConfiguration configuration)
    {
        if (!configuration.GetSection("AiConfig").Exists())
        {
            throw new ConfigurationMissingException("AiConfig is missing");
        }
        
        var apiKey = configuration.GetSection("AiConfig:ApiKey").Value!;
        var baseUrl = configuration.GetSection("AiConfig:BaseUrl").Value!;
        // var model = configuration.GetSection("AiConfig:Model").Value!;

        _chatClient = new ChatClient(CurrentModel, new ApiKeyCredential(apiKey), new OpenAIClientOptions
        {
            Endpoint = new Uri(baseUrl)
        }); 
        var client = new TogetherAIClient(new HttpClient(new LoggingHandler(new HttpClientHandler())));
    }

    public async Task<string?> SendTextMessage(CustomPrompt prompt, Dictionary<string, object>? responseFormat = null, CancellationToken ct = default)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(prompt.System),
            new UserChatMessage(prompt.User)
        };
        var result = await _chatClient.CompleteChatAsync(messages, _options, ct);
        return result.Value.Content[0].Text;
    }
    
    private readonly ChatCompletionOptions _options = new()
    {
        
        ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
    };
}