using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vivaro_webapi.Modules;

public static class KeyCloakConfigExtensions
{
    public static IServiceCollection ConfigureKeyCloak(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationOptions = configuration
            .GetSection(key: KeycloakAuthenticationOptions.Section)
            .Get<KeycloakAuthenticationOptions>();

        var authorizationOptions = configuration
            .GetSection(KeycloakProtectionClientOptions.Section)
            .Get<KeycloakProtectionClientOptions>();

        services.Configure<KeycloakProtectionClientOptions>(
            configuration.GetSection(KeycloakProtectionClientOptions.Section));
        services.Configure<KeycloakAuthenticationOptions>(
            configuration.GetSection(KeycloakAuthenticationOptions.Section));

        services.AddKeycloakAuthorization(authorizationOptions!);
        services.AddKeycloakWebApiAuthentication(configuration,
            (options) =>
            {
                options.RequireHttpsMetadata = false;
                options.Audience = authenticationOptions!.Resource;
            });
        services.AddAuthorization();

        return services;
    }
}