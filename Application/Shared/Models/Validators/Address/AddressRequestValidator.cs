using Application.Shared.Models.Request;
using FluentValidation;

namespace Application.Shared.Models.Validators.Address;

public class AddressRequestValidator : AbstractValidator<AddressRequest>
{
    public AddressRequestValidator()
    {
        
        RuleFor(u => u.Complement)
            .MaximumLength(100)
            .WithMessage("Complemento não pode ultrapassar 100 caracteres.");
        
        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("País não informado.")
            .MaximumLength(50)
            .WithMessage("País não pode ultrapassar 50 caracteres.");
        
        RuleFor(x => x.Neighborhood)
            .NotEmpty()
            .WithMessage("Bairro não informado.")
            .MaximumLength(100)
            .WithMessage("Bairro não pode ultrapassar 100 caracteres.");
        
        RuleFor(x => x.RecipienteName)
            .NotEmpty()
            .WithMessage("Nome do cliente não informado.")
            .MaximumLength(255)
            .WithMessage("Nome do cliente não pode ultrapassar 255 caracteres.");
        
        RuleFor(x => x.Number)
            .NotEmpty()
            .WithMessage("Número não informado.")
            .MaximumLength(50)
            .WithMessage("Número não pode ultrapassar 50 caracteres.");
        
        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Rua não informada.")
            .MaximumLength(200)
            .WithMessage("Nome da rua não pode ultrapassar 200 caracteres.");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("Cidade não informada.")
            .MaximumLength(100)
            .WithMessage("Cidade não pode ultrapassar 100 caracteres.");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("Estado não informado.")
            .MaximumLength(50)
            .WithMessage("Estado não pode ultrapassar 50 caracteres.");

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("Cep não informado.")
            .Matches(@"^\d{5}-\d{3}$")
            .WithMessage("Cep deve estar no formato 12345-678.");
    }
}