using Application.Shared.Notifications;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vivaro_webapi.Modules;

public static class UseCaseExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services, IConfiguration configuration) =>
        services.AddNotifications()
            .AddIndividualCustomerUseCase()
            .AddCorporateCustomerUseCase();

    private static IServiceCollection AddIndividualCustomerUseCase(this IServiceCollection services) =>
        services.AddCreateIndividualCustomerUseCase();


    private static IServiceCollection AddCorporateCustomerUseCase(this IServiceCollection services) =>
        services.AddCreateCorporateCustomerUseCase();
}