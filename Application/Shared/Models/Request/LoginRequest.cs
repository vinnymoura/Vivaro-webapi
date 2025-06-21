namespace Application.Shared.Models.Request;

public class LoginRequest
{
    public string Access { get; set; } = null!;
    public string Password { get; set; } = null!;
}