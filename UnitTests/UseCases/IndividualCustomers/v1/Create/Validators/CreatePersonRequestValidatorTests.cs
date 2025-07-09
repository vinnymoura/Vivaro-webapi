using FluentValidation.TestHelper;
using Xunit;
using Application.Shared.Models.Request;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Validators;
using Application.Shared.Enum;

namespace UnitTests.UseCases.IndividualCustomers.v1.Create.Validators;

public class CreatePersonRequestValidatorTests
{
    private readonly CreatePersonRequestValidator _validator = new();

    #region Email Tests

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_EmailIsEmpty(string email)
    {
        var request = new CreatePersonRequest { Email = email };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("O e-mail é obrigatório.");
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("user@.com")]
    [InlineData("user@domain")]
    [InlineData("user@domain,com")]
    public async Task ShouldHaveError_When_EmailIsInvalid(string email)
    {
        var request = new CreatePersonRequest { Email = email };
        var result = await _validator.TestValidateAsync(request);
    
        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("O e-mail fornecido não é válido.");
    }
    
    [Fact]
    public async Task ShouldHaveError_When_EmailHasConsecutiveDots()
    {
        var request = new CreatePersonRequest { Email = "user@domain..com" };
        var result = await _validator.TestValidateAsync(request);
    
        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("O e-mail não pode conter pontos consecutivos.");
    }

    #endregion

    #region Password Tests

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_PasswordIsEmpty(string password)
    {
        var request = new CreatePersonRequest { Password = password };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("A senha é obrigatória.");
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("abc")]
    public async Task ShouldHaveError_When_PasswordIsTooShort(string password)
    {
        var request = new CreatePersonRequest { Password = password };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("A senha deve ter no mínimo 8 caracteres.");
    }

    #endregion

    #region Phone Number Tests

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_PhoneNumberIsEmpty(string phoneNumber)
    {
        var request = new CreatePersonRequest { PhoneNumber = phoneNumber };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.PhoneNumber)
            .WithErrorMessage("O telefone é obrigatório.");
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
            .WithErrorMessage("O telefone deve conter 10 ou 11 dígitos.");
    }

    [Theory]
    [InlineData("1234567890")]
    [InlineData("12345678901")]
    public async Task ShouldNotHavePhoneNumberError_When_PhoneNumberIsValid(string phoneNumber)
    {
        var request = new CreatePersonRequest
        {
            Email = "test@example.com",
            Password = "password123",
            PhoneNumber = phoneNumber,
            PersonType = PersonType.NaturalPerson,
            FirstName = "Nome",
            LastName = "Sobrenome",
            Cpf = "12345678901",
            BirthDate = DateTime.Now.AddYears(-20)
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveValidationErrorFor(c => c.PhoneNumber);
    }

    #endregion

    #region NaturalPerson Tests

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_FirstNameIsEmpty_For_NaturalPerson(string firstName)
    {
        var request = new CreatePersonRequest 
        { 
            PersonType = PersonType.NaturalPerson,
            FirstName = firstName 
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.FirstName)
            .WithErrorMessage("O nome é obrigatório.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_LastNameIsEmpty_For_NaturalPerson(string lastName)
    {
        var request = new CreatePersonRequest 
        { 
            PersonType = PersonType.NaturalPerson,
            FirstName = "John", 
            LastName = lastName 
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.LastName)
            .WithErrorMessage("O sobrenome é obrigatório.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_CpfIsEmpty_For_NaturalPerson(string cpf)
    {
        var request = new CreatePersonRequest 
        { 
            PersonType = PersonType.NaturalPerson,
            Cpf = cpf 
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Cpf)
            .WithErrorMessage("O CPF é obrigatório para Pessoa Física.");
    }

    [Theory]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    public async Task ShouldHaveError_When_CpfIsInvalid_For_NaturalPerson(string cpf)
    {
        var request = new CreatePersonRequest 
        { 
            PersonType = PersonType.NaturalPerson,
            Cpf = cpf 
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Cpf)
            .WithErrorMessage("O CPF deve conter exatamente 11 dígitos.");
    }

    [Fact]
    public async Task ShouldHaveError_When_BirthDateIsInFuture_For_NaturalPerson()
    {
        var request = new CreatePersonRequest 
        { 
            PersonType = PersonType.NaturalPerson,
            BirthDate = DateTime.Now.AddDays(1) 
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.BirthDate)
            .WithErrorMessage("A data de nascimento deve ser anterior à data atual.");
    }

    [Fact]
    public async Task ShouldNotHaveBirthDateError_When_BirthDateIsInPast_For_NaturalPerson()
    {
        var request = new CreatePersonRequest
        {
            PersonType = PersonType.NaturalPerson,
            Email = "test@example.com",
            Password = "password123",
            PhoneNumber = "12345678901",
            FirstName = "Nome",
            LastName = "Sobrenome",
            Cpf = "12345678901",
            BirthDate = DateTime.Now.AddYears(-20)
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveValidationErrorFor(c => c.BirthDate);
    }

    #endregion

    #region LegalPerson Tests

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_CompanyNameIsEmpty_For_LegalPerson(string companyName)
    {
        var request = new CreatePersonRequest 
        { 
            PersonType = PersonType.LegalPerson,
            CompanyName = companyName 
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.CompanyName)
            .WithErrorMessage("O nome da empresa é obrigatório.");
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_TradeNameIsEmpty_For_LegalPerson(string tradeName)
    {
        var request = new CreatePersonRequest 
        { 
            PersonType = PersonType.LegalPerson,
            TradeName = tradeName 
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.TradeName)
            .WithErrorMessage("O nome fantasia é obrigatório.");
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldHaveError_When_CnpjIsEmpty_For_LegalPerson(string cnpj)
    {
        var request = new CreatePersonRequest 
        { 
            PersonType = PersonType.LegalPerson,
            Cnpj = cnpj 
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Cnpj)
            .WithErrorMessage("O CNPJ é obrigatório para Pessoa Jurídica.");
    }

    [Theory]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    [InlineData("abcdefghijk")]
    public async Task ShouldHaveError_When_CnpjIsInvalid_For_LegalPerson(string cnpj)
    {
        var request = new CreatePersonRequest 
        { 
            PersonType = PersonType.LegalPerson,
            Cnpj = cnpj 
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.Cnpj)
            .WithErrorMessage("O CNPJ deve conter exatamente 14 dígitos.");
    }

    #endregion

    #region PersonType Tests

    [Fact]
    public async Task ShouldHaveError_When_PersonTypeIsInvalid()
    {
        var request = new CreatePersonRequest 
        { 
            PersonType = (PersonType)999 
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(c => c.PersonType)
            .WithErrorMessage("O tipo de pessoa deve ser 'Pessoa Física' ou 'Pessoa Jurídica'.");
    }

    #endregion

    #region Address Tests

    [Fact]
    public async Task ShouldValidateAddress_When_AddressIsProvided()
    {
        var request = new CreatePersonRequest
        {
            Address = new AddressRequest { ZipCode = "invalid" }
        };
        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor("Address.ZipCode");
    }

    #endregion

    #region Valid Request Tests

    [Fact]
    public async Task ShouldNotHaveErrors_When_NaturalPersonRequestIsValid()
    {
        var request = new CreatePersonRequest
        {
            PersonType = PersonType.NaturalPerson,
            Email = "test@example.com",
            Password = "P@ssword123",
            PhoneNumber = "12345678901",
            FirstName = "Nome Completo",
            LastName = "Sobrenome Completo",
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

    [Fact]
    public async Task ShouldNotHaveErrors_When_LegalPersonRequestIsValid()
    {
        var request = new CreatePersonRequest
        {
            PersonType = PersonType.LegalPerson,
            Email = "company@example.com",
            Password = "P@ssword123",
            PhoneNumber = "12345678901",
            CompanyName = "Empresa Exemplo Ltda",
            TradeName = "Empresa Exemplo",
            Cnpj = "12345678901234",
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

    #endregion
}