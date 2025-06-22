using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Validators;
using FluentValidation.TestHelper;
using Xunit;
using Application.Shared.Models.Request;

namespace UnitTests.UseCases.IndividualCustomers.v1.Create.Validators;

public class CreateIndividualCustomerRequestValidatorTests
{
    private readonly CreateIndividualCustomerRequestValidator _validator = new();
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ShouldHaveError_When_NameIsEmpty(string name)
    {
        var request = new CreateIndividualCustomerRequest { Name = name };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Nome é obrigatório.");
    }
    
    [Fact]
    public async Task ShouldHaveError_When_LoginIsNull()
    {
        var request = new CreateIndividualCustomerRequest { Login = null! };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Login)
            .WithErrorMessage("Login é obrigatório.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ShouldHaveError_When_PhoneNumberIsEmpty(string phoneNumber)
    {
        var request = new CreateIndividualCustomerRequest { PhoneNumber = phoneNumber };
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
        var request = new CreateIndividualCustomerRequest { PhoneNumber = phoneNumber };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.PhoneNumber)
            .WithErrorMessage("Telefone deve conter 10 ou 11 dígitos.");
    }
    
    [Theory]
    [InlineData("1234567890")]
    [InlineData("12345678901")]
    public async Task ShouldNotHavePhoneNumberError_When_PhoneNumberIsValid(string phoneNumber)
    {
        var request = new CreateIndividualCustomerRequest 
        { 
            Name = "Nome", 
            PhoneNumber = phoneNumber,
            Cpf = "12345678901",
            Login = new LoginRequest { Access = "usuario", Password = "Senha@123" }
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveValidationErrorFor(c => c.PhoneNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ShouldHaveError_When_CpfIsEmpty(string cpf)
    {
        var request = new CreateIndividualCustomerRequest { Cpf = cpf };
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
        var request = new CreateIndividualCustomerRequest { Cpf = cpf };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Cpf)
            .WithErrorMessage("CPF deve conter 11 dígitos numéricos.");
    }
    
    [Fact]
    public async Task ShouldHaveError_When_BirthDateIsInFuture()
    {
        var request = new CreateIndividualCustomerRequest { BirthDate = DateTime.Now.AddDays(1) };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.BirthDate)
            .WithErrorMessage("Data de nascimento deve ser no passado.");
    }
    
    [Fact]
    public async Task ShouldNotHaveBirthDateError_When_BirthDateIsInPast()
    {
        var request = new CreateIndividualCustomerRequest 
        { 
            Name = "Nome", 
            PhoneNumber = "12345678901",
            Cpf = "12345678901",
            BirthDate = DateTime.Now.AddYears(-20),
            Login = new LoginRequest { Access = "usuario", Password = "Senha@123" }
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveValidationErrorFor(c => c.BirthDate);
    }
    
    [Fact]
    public async Task ShouldHaveLoginAccessError_When_AccessIsEmpty()
    {
        var request = new CreateIndividualCustomerRequest 
        { 
            Login = new LoginRequest { Access = "", Password = "Senha@123" }
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor("Login.Access")
            .WithErrorMessage("Username não informado.");
    }
    
    [Fact]
    public async Task ShouldHaveLoginAccessError_When_AccessIsTooLong()
    {
        var longUsername = new string('a', 151);
        var request = new CreateIndividualCustomerRequest 
        { 
            Login = new LoginRequest { Access = longUsername, Password = "Senha@123" }
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor("Login.Access")
            .WithErrorMessage("Username não pode ultrapassar 150 caracteres.");
    }
    
    [Fact]
    public async Task ShouldHaveLoginPasswordError_When_PasswordIsEmpty()
    {
        var request = new CreateIndividualCustomerRequest 
        { 
            Login = new LoginRequest { Access = "usuario", Password = "" }
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor("Login.Password")
            .WithErrorMessage("Senha não informada.");
    }
    
    [Theory]
    [InlineData("abcdefgh", "Senha deve conter pelo menos uma letra maiúscula.")]
    [InlineData("ABCDEFGH", "Senha deve conter pelo menos uma letra minúscula.")]
    [InlineData("Abcdefgh", "Senha deve conter pelo menos um número.")]
    [InlineData("Abcdefg1", "Senha deve conter pelo menos um caractere especial.")]
    public async Task ShouldHaveLoginPasswordError_When_PasswordDoesNotMeetRequirements(string password, string errorMessage)
    {
        var request = new CreateIndividualCustomerRequest 
        { 
            Login = new LoginRequest { Access = "vmmoura482@gmail.com", Password = password }
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor("Login.Password")
            .WithErrorMessage(errorMessage);
    }
    
    [Fact]
    public async Task ShouldValidateAddress_When_AddressIsProvided()
    {
        var request = new CreateIndividualCustomerRequest 
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
        var request = new CreateIndividualCustomerRequest 
        { 
            Name = "Nome Completo",
            PhoneNumber = "12345678901",
            Cpf = "12345678901",
            BirthDate = DateTime.Now.AddYears(-20),
            Login = new LoginRequest { Access = "usuario", Password = "Senha@123" },
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