using Application.Shared.Notifications;
using Xunit;

namespace UnitTests.shared.Notifications;

public class NotificationTests
{
    private readonly Notification _notification = new();
    
    [Theory]
    [InlineData("Key", "Message")]
    public void AddErrorMessage_ShouldAddErrorMessage(string key, string message)
    {
        // Act
        _notification.AddErrorMessage(key, message);

        // Assert
        Assert.True(_notification.GetErrorMessages().ContainsKey(key));
        Assert.Contains(message, _notification.GetErrorMessages()[key]);
    }
    
    [Fact]
    public void AddErrorMessages_WhenValidationResultIsValid_ShouldNotAddErrorMessage()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();

        // Act
        _notification.AddErrorMessages(validationResult);

        // Assert
        Assert.False(_notification.IsInvalid);
    }
    
    [Fact]
    public void AddErrorMessages_WhenValidationResultIsNotValid_ShouldAddErrorMessage()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
        {
            new("Key", "Message")
        });

        // Act
        _notification.AddErrorMessages(validationResult);

        // Assert
        Assert.True(_notification.IsInvalid);
        Assert.True(_notification.GetErrorMessages().ContainsKey("Key"));
        Assert.Contains("Message", _notification.GetErrorMessages()["Key"]);
    }
    
    [Fact]
    public void ToString_ShouldReturnAllErrorMessages()
    {
        // Arrange
        _notification.AddErrorMessage("Key1", "Message1");
        _notification.AddErrorMessage("Key2", "Message2");

        // Act
        var result = _notification.ToString();

        // Assert
        Assert.Contains("Message1", result);
        Assert.Contains("Message2", result);
    }
}