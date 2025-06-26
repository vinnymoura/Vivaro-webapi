using Application.Shared.Models.Validators;
using Application.Shared.Models.Validators.Address;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Models;
using FluentValidation;

namespace Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Validators;

public class CreateCorporateCustomerRequestValidator : AbstractValidator<CreateCorporateCustomerRequest>
{
    public CreateCorporateCustomerRequestValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage("Nome da empresa é obrigatório.");

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

        RuleFor(x => x.Cnpj)
            .NotEmpty()
            .WithMessage("CNPJ é obrigatório.")
            .Matches(@"^\d{14}$")
            .WithMessage("CNPJ deve conter 14 dígitos numéricos.");
    }
}