using FluentValidation.TestHelper;
using Xunit;
using Application.Shared.Models.Request;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Validators;

namespace UnitTests.UseCases.IndividualCustomers.v1.Create.Validators;

public class CreateIndividualCustomerRequestValidatorTests
{
    private readonly CreateIndividualCustomerRequestValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_FirstNameIsEmpty(string firstName)
    {
        var request = new CreatePersonRequest { FirstName = firstName };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.FirstName)
            .WithErrorMessage("Nome é obrigatório.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_LastNameIsEmpty(string lastName)
    {
        var request = new CreatePersonRequest { FirstName = "John", LastName = lastName};
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.LastName)
            .WithErrorMessage("Sobrenome é obrigatório.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_PhoneNumberIsEmpty(string phoneNumber)
    {
        var request = new CreatePersonRequest { PhoneNumber = phoneNumber };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.PhoneNumber)
            .WithErrorMessage("Telefone é obrigatório.");
    }

    [Theory]
    [InlineData("123456789")]
    [InlineData("123456789012")]
    [InlineData("abc1234567")]
    public async Task ShouldHaveError_When_PhoneNumberIsInvalid(string phoneNumber)
    {
        var request = new CreatePersonRequest { PhoneNumber = phoneNumber };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.PhoneNumber)
            .WithErrorMessage("Telefone deve conter 10 ou 11 dígitos.");
    }

    [Theory]
    [InlineData("1234567890")]
    [InlineData("12345678901")]
    public async Task ShouldNotHavePhoneNumberError_When_PhoneNumberIsValid(string phoneNumber)
    {
        var request = new CreatePersonRequest
        {
            FirstName = "Nome",
            PhoneNumber = phoneNumber,
            Cpf = "12345678901"
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveValidationErrorFor(c => c.PhoneNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_CpfIsEmpty(string cpf)
    {
        var request = new CreatePersonRequest { Cpf = cpf };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Cpf)
            .WithErrorMessage("CPF é obrigatório.");
    }

    [Theory]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    [InlineData("abcdefghijk")]
    public async Task ShouldHaveError_When_CpfIsInvalid(string cpf)
    {
        var request = new CreatePersonRequest { Cpf = cpf };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Cpf)
            .WithErrorMessage("CPF deve conter 11 dígitos numéricos.");
    }

    [Fact]
    public async Task ShouldHaveError_When_BirthDateIsInFuture()
    {
        var request = new CreatePersonRequest { BirthDate = DateTime.Now.AddDays(1) };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.BirthDate)
            .WithErrorMessage("Data de nascimento deve ser no passado.");
    }

    [Fact]
    public async Task ShouldNotHaveBirthDateError_When_BirthDateIsInPast()
    {
        var request = new CreatePersonRequest
        {
            FirstName = "Nome",
            PhoneNumber = "12345678901",
            Cpf = "12345678901",
            BirthDate = DateTime.Now.AddYears(-20),
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveValidationErrorFor(c => c.BirthDate);
    }

    [Fact]
    public async Task ShouldValidateAddress_When_AddressIsProvided()
    {
        var request = new CreatePersonRequest
        {
            Address = new AddressRequest { ZipCode = "invalid" }
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor("Address.ZipCode")
            .WithErrorMessage("Cep deve estar no formato 12345-678.");
    }

    [Fact]
    public async Task ShouldNotHaveErrors_When_RequestIsValid()
    {
        var request = new CreatePersonRequest
        {
            FirstName = "Nome Completo",
            LastName = "Sobrenome Completo",
            PhoneNumber = "12345678901",
            Cpf = "12345678901",
            BirthDate = DateTime.Now.AddYears(-20),
            Address = new AddressRequest
            {
                Street = "Rua Exemplo",
                Number = "123",
                Neighborhood = "Bairro",
                City = "Cidade",
                State = "Estado",
                Country = "País",
                ZipCode = "12345-678",
                RecipienteName = "Nome do Destinatário"
            }
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}