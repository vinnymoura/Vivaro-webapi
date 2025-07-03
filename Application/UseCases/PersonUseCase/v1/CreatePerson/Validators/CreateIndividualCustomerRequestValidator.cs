using Application.Shared.Enum;
using Application.Shared.Models.Validators.Address;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;
using FluentValidation;

namespace Application.UseCases.PersonUseCase.v1.CreatePerson.Validators;

public class CreateIndividualCustomerRequestValidator : AbstractValidator<CreatePersonRequest>
{
    public CreateIndividualCustomerRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail fornecido não é válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.");
        // Adicione outras regras de complexidade de senha aqui se necessário.

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("O telefone é obrigatório.")
            .Matches(@"^\d{10,11}$").WithMessage("O telefone deve conter 10 ou 11 dígitos.");

        // Validação do endereço, se ele for fornecido (comum a ambos)
        RuleFor(x => x.Address)
            .SetValidator(new AddressRequestValidator()!) // Reutiliza seu validador de endereço
            .When(x => x.Address != null);

        When(x => x.PersonType == PersonType.NaturalPerson, () =>
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("O nome é obrigatório para Pessoa Física.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("O sobrenome é obrigatório para Pessoa Física.");

            RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("O CPF é obrigatório para Pessoa Física.")
                .Length(11).WithMessage("O CPF deve conter exatamente 11 dígitos.")
                .Must(BeAValidCpf).WithMessage("O CPF fornecido é inválido."); // Validação de algoritmo

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("A data de nascimento é obrigatória para Pessoa Física.")
                .LessThan(DateTime.Now).WithMessage("A data de nascimento deve ser no passado.");
        });

        When(x => x.PersonType == PersonType.LegalPerson, () =>
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("A Razão Social é obrigatória para Pessoa Jurídica.");

            RuleFor(x => x.Cnpj)
                .NotEmpty().WithMessage("O CNPJ é obrigatório para Pessoa Jurídica.")
                .Length(14).WithMessage("O CNPJ deve conter exatamente 14 dígitos.")
                .Must(BeAValidCnpj).WithMessage("O CNPJ fornecido é inválido."); // Validação de algoritmo
        });
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