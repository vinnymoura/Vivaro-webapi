using Application.Shared.Models.Request;
using FluentValidation;

namespace Application.Shared.Models.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Access)
            .NotEmpty()
            .WithMessage("Username não informado.")
            .MaximumLength(150)
            .WithMessage("Username não pode ultrapassar 150 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Senha não informada.")
            .MinimumLength(8)
            .WithMessage(" Senha deve ter pelo menos 8 caracteres.")
            .Matches(@"[A-Z]")
            .WithMessage("Senha deve conter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]")
            .WithMessage("Senha deve conter pelo menos uma letra minúscula.")
            .Matches(@"[0-9]")
            .WithMessage("Senha deve conter pelo menos um número.")
            .Matches(@"[\W_]")
            .WithMessage("Senha deve conter pelo menos um caractere especial.");
    }
}