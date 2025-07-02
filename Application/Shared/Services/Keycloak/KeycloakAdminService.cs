using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Application.Shared.Entities;
using Application.Shared.Notifications;
using Application.Shared.Services.Keycloak.Abstractions;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Shared.Services.Keycloak;

public class KeycloakAdminService(
    HttpClient httpClient,
    IConfiguration configuration,
    IHttpContextAccessor httpContextAccessor,
    IOptions<KeycloakProtectionClientOptions> keycloakOptions,
    Notification notification) : IKeycloakAdminService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly KeycloakProtectionClientOptions _keycloakOptions = keycloakOptions.Value;
    private readonly HttpClient _httpClient = httpClient;
    
    public async Task<string?> CreateUserAsync(UserKeycloak userKeycloak, CancellationToken cancellationToken)
    {
        var accessToken = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString()
            .Replace("Bearer ", "");

        if (string.IsNullOrEmpty(accessToken))
        {
            notification.AddErrorMessage("Token", "Token de autenticação não encontrado ou inválido.");
            return null;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var endpoint = GetBaseEndpoint();

        var response = await _httpClient.PostAsJsonAsync(endpoint + "/users", userKeycloak, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var errorMessage = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent)?["errorMessage"];
            notification.AddErrorMessage("Keycloak", errorMessage ?? "Erro desconhecido.");
            return null;
        }

        var location = response.Headers.Location;
        return location != null ? location.ToString().Split('/').Last() : null;
    }

    private string GetBaseEndpoint()
    {
        var baseUrl = _keycloakOptions.AuthServerUrl.TrimEnd('/');
        var realm = _keycloakOptions.Realm;
        var endpoint = $"{baseUrl}/admin/realms/{realm}";
        return endpoint;
    }

    public async Task DeleteUserAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateUserAsync(string userId, string username, string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetUserIdByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}