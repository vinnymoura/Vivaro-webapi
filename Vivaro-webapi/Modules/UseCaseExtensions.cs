using Application.Shared.Notifications;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vivaro_webapi.Modules;

public static class UseCaseExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services, IConfiguration configuration) =>
        services.AddNotifications()
            .AddUsersUseCase(configuration);

    private static IServiceCollection AddUsersUseCase(this IServiceCollection services, IConfiguration configuration) =>
        services.AddCreateIndividualCustomerUseCase(configuration);
}