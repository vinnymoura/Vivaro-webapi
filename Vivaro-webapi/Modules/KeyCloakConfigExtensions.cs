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

        services.AddKeycloakAuthentication(authenticationOptions!);
        services.AddKeycloakAuthorization(authorizationOptions!);

        return services;
    }
}