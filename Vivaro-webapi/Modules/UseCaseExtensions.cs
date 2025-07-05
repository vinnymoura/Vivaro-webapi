using Application.Shared.Notifications;
using Application.UseCases.PersonUseCase.v1.CreatePerson;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vivaro_webapi.Modules;

public static class UseCaseExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services, IConfiguration configuration) =>
        services.AddNotifications()
            .AddPersonUseCase();

    private static IServiceCollection AddPersonUseCase(this IServiceCollection services) =>
        services.AddCreatePersonUseCase();
}