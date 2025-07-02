using Application.Shared.Entities;

namespace Application.Shared.Services.Keycloak.Abstractions;

public interface IKeycloakAdminService
{
    Task<string?> CreateUserAsync(UserKeycloak userKeycloak, CancellationToken cancellationToken);
    Task DeleteUserAsync(string userId, CancellationToken cancellationToken);
    Task UpdateUserAsync(string userId, string username, string email, CancellationToken cancellationToken);
    Task<string> GetUserIdByUsernameAsync(string username, CancellationToken cancellationToken);
}