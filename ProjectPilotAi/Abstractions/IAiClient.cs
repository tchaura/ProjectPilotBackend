using Common.Models;
using Together.AI;

namespace ProjectPilot.Abstractions;

public interface IAiClient
{
    public Task<string?> SendTextMessage(
        CustomPrompt prompt,
        Dictionary<string, object>? responseFormat = null,
        CancellationToken ct = default);
}