using Application.Shared.Interfaces.CpfValidator;
using Application.Shared.Models.Validators;
using Application.Shared.Models.Validators.Address;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;
using FluentValidation;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Validators;

public class CreateIndividualCustomerRequestValidator : AbstractValidator<CreateIndividualCustomerRequest>
{
    public CreateIndividualCustomerRequestValidator(ICpfValidatorService cpfValidatorService)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Nome é obrigatório.");

        RuleFor(x => x.Address)
            .SetValidator(new AddressRequestValidator()!)
            .When(x => x.Address != null);

        RuleFor(x => x.Login)
            .NotNull()
            .WithMessage("Login é obrigatório.")
            .SetValidator(new LoginRequestValidator());

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Telefone é obrigatório.")
            .Matches(@"^\d{10,11}$")
            .WithMessage("Telefone deve conter 10 ou 11 dígitos.");

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .WithMessage("CPF é obrigatório.")
            .MustAsync(async (cpf, cancellation) => await cpfValidatorService.IsValidCpfAsync(cpf))
            .WithMessage("CPF inválido na Receita Federal.");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now)
            .When(x => x.BirthDate.HasValue)
            .When(x => x.BirthDate != default(DateTime))
            .WithMessage("Data de nascimento deve ser no passado.");
    }
}