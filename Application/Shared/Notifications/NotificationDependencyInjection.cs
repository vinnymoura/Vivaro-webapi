using Microsoft.Extensions.DependencyInjection;

namespace Application.Shared.Notifications;

public static class NotificationDependencyInjection
{
    public static IServiceCollection AddNotifications(this IServiceCollection services)
        => services.AddScoped<Notification>();
}