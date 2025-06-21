using FluentValidation.Results;

namespace Application.Shared.Notifications;

public sealed class Notification
{
    private readonly Dictionary<string, IList<string>> _errorMessages = [];

    public bool IsInvalid => _errorMessages.Count != 0;

    public Dictionary<string, IList<string>> GetErrorMessages() => _errorMessages;

    public void AddErrorMessage(string key, string message)
    {
        if (!_errorMessages.TryGetValue(key, out var value))
        {
            value = new List<string>();
            _errorMessages[key] = value;
        }

        value.Add(message);
    }

    public override string ToString() => string.Join(' ', _errorMessages.SelectMany(x => x.Value));

    public void AddErrorMessages(ValidationResult result)
    {
        if (result.IsValid)
            return;

        foreach (var error in result.Errors)
        {
            AddErrorMessage(error.PropertyName, error.ErrorMessage);
        }
    }
}