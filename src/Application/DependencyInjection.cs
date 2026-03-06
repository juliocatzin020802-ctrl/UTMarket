using Microsoft.Extensions.DependencyInjection;
using UTMarket.Core.UseCases;
using UTMarket.Application;

namespace UTMarket.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Product Use Cases
        services.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCaseImpl>();
        services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCaseImpl>();
        services.AddScoped<IFindProductsUseCase, FindProductsUseCaseImpl>();
        services.AddScoped<ICreateProductUseCase, CreateProductUseCaseImpl>();
        services.AddScoped<IUpdateProductUseCase, UpdateProductUseCaseImpl>();
        services.AddScoped<IUpdateProductStockUseCase, UpdateProductStockUseCaseImpl>();
        services.AddScoped<ILowStockAlertUseCase, LowStockAlertUseCaseImpl>();

        // Sale Use Cases
        services.AddScoped<IFetchAllSalesUseCase, FetchAllSalesUseCaseImpl>();
        services.AddScoped<IFetchSaleByIdUseCase, FetchSaleByIdUseCaseImpl>();
        services.AddScoped<IFetchSalesByFilterUseCase, FetchSalesByFilterUseCaseImpl>();
        services.AddScoped<ICreateSaleUseCase, CreateSaleUseCaseImpl>();
        services.AddScoped<IUpdateSaleStatusUseCase, UpdateSaleStatusUseCaseImpl>();
        services.AddScoped<IRegisterSaleUseCase, RegisterSaleUseCaseImpl>();

        // Customer Use Cases
        services.AddScoped<IRegisterCustomerUseCase, RegisterCustomerUseCaseImpl>();
        services.AddScoped<IGetAllCustomersUseCase, GetAllCustomersUseCaseImpl>();

        return services;
    }
}
