using ProjectPilotWeb.Services;

namespace ProjectPilotWeb;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddProjectPilotServices(this IServiceCollection services)
    {
        services.AddSingleton<JiraService>();
        services.AddSingleton<AiService>();
        services.AddSingleton<PromptService>();
        services.AddSingleton<ResponseFormatService>();
        return services;
    }
}