using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UTMarket.Core.Abstractions;
using UTMarket.Infrastructure.Data;
using UTMarket.Infrastructure.Settings;

namespace UTMarket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure DatabaseSettings from the "ConnectionStrings" section
        services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));
        
        // Register the connection factory as a singleton
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

        // Here you would register your concrete repository implementations
        services.AddScoped<Core.Repositories.IProductRepository, Repositories.ProductRepository>();
        services.AddScoped<Core.Repositories.ISaleRepository, Repositories.SaleRepository>();
        services.AddScoped<Core.Repositories.ICustomerRepository, Repositories.SqlCustomerRepository>();

        return services;
    }
}
