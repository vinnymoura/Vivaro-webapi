using Application.Shared.Enum;
using Application.Shared.Models.Validators.Address;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;
using FluentValidation;

namespace Application.UseCases.PersonUseCase.v1.CreatePerson.Validators;

public class CreatePersonRequestValidator : AbstractValidator<CreatePersonRequest>
{
    public CreatePersonRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .MaximumLength(256).WithMessage("O e-mail não pode exceder 256 caracteres.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("O e-mail fornecido não é válido.")
            .Must(NotHaveConsecutiveDots).WithMessage("O e-mail não pode conter pontos consecutivos.")
            .Must(NotStartOrEndWithDot).WithMessage("O e-mail não pode começar ou terminar com ponto.")
            .EmailAddress().WithMessage("O e-mail fornecido não é válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*&])[A-Za-z\d@$!%*?&.]{8,}$")
            .WithMessage("A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um dígito e um caractere especial.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("O telefone é obrigatório.")
            .Matches(@"^\d{10,11}$").WithMessage("O telefone deve conter 10 ou 11 dígitos.");

        // Validação do endereço, se ele for fornecido (comum a ambos)
        RuleFor(x => x.Address)
            .SetValidator(new AddressRequestValidator()!) // Reutiliza seu validador de endereço
            .When(x => x.Address != null);

        // Validação específica para Pessoa Física e Pessoa Jurídica
        RuleFor(x => x.PersonType)
            .IsInEnum().WithMessage("O tipo de pessoa deve ser 'Pessoa Física' ou 'Pessoa Jurídica'.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .When(x => x.PersonType == PersonType.NaturalPerson);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("O sobrenome é obrigatório.")
            .When(x => x.PersonType == PersonType.NaturalPerson);

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .LessThan(DateTime.Now).WithMessage("A data de nascimento deve ser anterior à data atual.")
            .When(x => x.PersonType == PersonType.NaturalPerson);

        // Validação condicional baseada no tipo de pessoa
        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório para Pessoa Física.")
            .Length(11).WithMessage("O CPF deve conter exatamente 11 dígitos.")
            .Must(BeAValidCpf).WithMessage("O CPF fornecido é inválido.") // Validação de algoritmo
            .When(x => x.PersonType == PersonType.NaturalPerson);

        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("O nome da empresa é obrigatório.")
            .When(x => x.PersonType == PersonType.LegalPerson);

        RuleFor(x => x.TradeName)
            .NotEmpty().WithMessage("O nome fantasia é obrigatório.")
            .When(x => x.PersonType == PersonType.LegalPerson);

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("O CNPJ é obrigatório para Pessoa Jurídica.")
            .Length(14).WithMessage("O CNPJ deve conter exatamente 14 dígitos.")
            .Must(BeAValidCnpj).WithMessage("O CNPJ fornecido é inválido.") // Validação de algoritmo
            .When(x => x.PersonType == PersonType.LegalPerson);
    }

    private static bool NotHaveConsecutiveDots(string email)
    {
        return !string.IsNullOrEmpty(email) && !email.Contains("..");
    }

    private static bool NotStartOrEndWithDot(string email)
    {
        if (string.IsNullOrEmpty(email)) return false;

        var parts = email.Split('@');
        if (parts.Length != 2) return false;

        var localPart = parts[0];
        var domainPart = parts[1];

        return !localPart.StartsWith($".") && !localPart.EndsWith($".") &&
               !domainPart.StartsWith($".") && !domainPart.EndsWith($".");
    }

    private static bool BeAValidCpf(string cpf)
    {
        return !string.IsNullOrEmpty(cpf) && cpf.All(char.IsDigit);
    }

    private static bool BeAValidCnpj(string cnpj)
    {
        return !string.IsNullOrEmpty(cnpj) && cnpj.All(char.IsDigit);
    }
}