using Application.Shared.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace UnitTests.shared.Notifications;

public class NotificationDependencyInjectionTests
{
    [Fact]
    public void Should_Get_Notification()
    {
        var services = new ServiceCollection();

        services.AddNotifications();

        var provider = services.BuildServiceProvider();

        var notification = provider.GetService<Notification>();

        Assert.NotNull(notification);
    }
}