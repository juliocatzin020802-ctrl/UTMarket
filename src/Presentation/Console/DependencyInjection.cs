using Microsoft.Extensions.DependencyInjection;
using UTMarket.Presentation.Console.Orchestrators;

namespace UTMarket.Presentation.Console;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<SalesHistoryOrchestrator>();
        // Add other presentation-level services here
        return services;
    }
}
