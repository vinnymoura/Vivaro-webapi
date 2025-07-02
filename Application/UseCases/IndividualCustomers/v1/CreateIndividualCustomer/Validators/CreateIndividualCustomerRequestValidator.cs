using Application.Shared.Models.Validators;
using Application.Shared.Models.Validators.Address;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;
using FluentValidation;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Validators;

public class CreateIndividualCustomerRequestValidator : AbstractValidator<CreateIndividualCustomerRequest>
{
    public CreateIndividualCustomerRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Nome é obrigatório.");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Sobrenome é obrigatório.");

        RuleFor(x => x.Address)
            .SetValidator(new AddressRequestValidator()!)
            .When(x => x.Address != null);
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Telefone é obrigatório.")
            .Matches(@"^\d{10,11}$")
            .WithMessage("Telefone deve conter 10 ou 11 dígitos.");

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .WithMessage("CPF é obrigatório.")
            .Matches(@"^\d{11}$")
            .WithMessage("CPF deve conter 11 dígitos numéricos.");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now)
            .When(x => x.BirthDate.HasValue)
            .When(x => x.BirthDate != default(DateTime))
            .WithMessage("Data de nascimento deve ser no passado.");
    }
}