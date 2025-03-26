using Microsoft.Extensions.DependencyInjection;
using ProjectPilot.Abstractions;

namespace ProjectPilot;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddAiClient(this IServiceCollection services)
    {
        // services.AddSingleton<IAiClient, OpenAiClient>();
        services.AddSingleton<IAiClient, TogetherXyzAiClient>();
        return services;
    }
}