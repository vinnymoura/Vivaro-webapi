using Application.Shared.Entities;
using Application.Shared.Enum;
using Application.Shared.Services.Abstractions;
using Application.Shared.Services.Keycloak.Abstractions;
using Application.UseCases.PersonUseCase.v1.CreatePerson;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Abstractions;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Services.Repositories.Abstractions;
using AutoFixture;
using Moq;
using Xunit;

namespace UnitTests.UseCases.IndividualCustomers.v1.Create;

public class CreatePersonUseCaseTests
{
    private readonly Mock<IPersonRepository> _mockRepository = new();
    private readonly Mock<IOutputPort> _mockOutputPort = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Fixture _fixture = new();
    private readonly Mock<IKeycloakAdminService> _mockKeycloakAdminService = new();
    private readonly CreatePersonUseCase _service;

    public CreatePersonUseCaseTests()
    {
        _service = new CreatePersonUseCase(_mockRepository.Object,
            _mockKeycloakAdminService.Object,
            _mockUnitOfWork.Object);
        _service.SetOutputPort(_mockOutputPort.Object);
    }

    [Fact]
    public async Task CreateIndividualCustomer_When_KeycloakCreationFails_Should_CallKeycloakCreationFailed()
    {
        // Arrange
        var request = _fixture.Create<CreatePersonRequest>();
        request.PersonType = PersonType.NaturalPerson; // Definindo explicitamente o tipo de pessoa
        _mockRepository.Setup(m => m.IndividualCustomerExists(request.Cpf!)).ReturnsAsync(false);
        _mockKeycloakAdminService.Setup(k => k.CreateUserAsync(It.IsAny<UserKeycloak>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(string.Empty); // Simulate Keycloak creation failure

        // Act
        await _service.ExecuteAsync(request, CancellationToken.None);

        // Assert
        _mockOutputPort.Verify(o => o.KeycloakCreationFailed(), Times.Once);
        _mockOutputPort.Verify(o => o.NaturalPersonCreated(It.IsAny<NaturalPerson>()), Times.Never);
        _mockOutputPort.Verify(o => o.NaturalPersonAlreadyExists(), Times.Never);
        _mockRepository.Verify(r => r.CreatePerson(It.IsAny<NaturalPerson>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateIndividualCustomer_When_CustomerExists_Should_CallCustomerAlreadyExists()
    {
        // Arrange
        var request = _fixture.Create<CreatePersonRequest>();
        request.PersonType = PersonType.NaturalPerson;
        _mockRepository.Setup(m => m.IndividualCustomerExists(request.Cpf!)).ReturnsAsync(true);

        // Act
        await _service.ExecuteAsync(request, CancellationToken.None);

        // Assert
        _mockOutputPort.Verify(o => o.NaturalPersonAlreadyExists(), Times.Once);
        _mockOutputPort.Verify(o => o.NaturalPersonCreated(It.IsAny<NaturalPerson>()), Times.Never);
        _mockRepository.Verify(
            r => r.CreatePerson(It.IsAny<NaturalPerson>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateIndividualCustomer_When_CustomerDoesNotExist_Should_CreateCustomer()
    {
        // Arrange
        var request = _fixture.Create<CreatePersonRequest>();
        request.PersonType = PersonType.NaturalPerson;
        _mockRepository.Setup(m => m.IndividualCustomerExists(request.Cpf!)).ReturnsAsync(false);
        _mockKeycloakAdminService.Setup(k => k.CreateUserAsync(It.IsAny<UserKeycloak>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("user-id-123"); // Simula criação bem-sucedida

        // Act
        await _service.ExecuteAsync(request, CancellationToken.None);

        // Assert
        _mockOutputPort.Verify(o => o.NaturalPersonAlreadyExists(), Times.Never);
        _mockOutputPort.Verify(o => o.NaturalPersonCreated(It.IsAny<NaturalPerson>()), Times.Once);
        _mockRepository.Verify(
            r => r.CreatePerson(It.Is<NaturalPerson>(p => p.KeycloakId == "user-id-123"),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateLegalPerson_When_KeycloakCreationFails_Should_CallKeycloakCreationFailed()
    {
        // Arrange
        var request = _fixture.Create<CreatePersonRequest>();
        request.PersonType = PersonType.LegalPerson;
        _mockRepository.Setup(m => m.LegalCustomerExists(request.Cnpj!)).ReturnsAsync(false);
        _mockKeycloakAdminService.Setup(k => k.CreateUserAsync(It.IsAny<UserKeycloak>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(string.Empty); // Simulate Keycloak creation failure

        // Act
        await _service.ExecuteAsync(request, CancellationToken.None);

        // Assert
        _mockOutputPort.Verify(o => o.KeycloakCreationFailed(), Times.Once);
        _mockOutputPort.Verify(o => o.LegalPersonCreated(It.IsAny<LegalPerson>()), Times.Never);
        _mockOutputPort.Verify(o => o.LegalPersonAlreadyExists(), Times.Never);
        _mockRepository.Verify(r => r.CreatePerson(It.IsAny<LegalPerson>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateLegalPerson_When_CustomerExists_Should_CallCustomerAlreadyExists()
    {
        // Arrange
        var request = _fixture.Create<CreatePersonRequest>();
        request.PersonType = PersonType.LegalPerson;
        _mockRepository.Setup(m => m.LegalCustomerExists(request.Cnpj!)).ReturnsAsync(true);

        // Act
        await _service.ExecuteAsync(request, CancellationToken.None);

        // Assert
        _mockOutputPort.Verify(o => o.LegalPersonAlreadyExists(), Times.Once);
        _mockOutputPort.Verify(o => o.LegalPersonCreated(It.IsAny<LegalPerson>()), Times.Never);
        _mockRepository.Verify(
            r => r.CreatePerson(It.IsAny<LegalPerson>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateLegalPerson_When_CustomerDoesNotExist_Should_CreateCustomer()
    {
        // Arrange
        var request = _fixture.Create<CreatePersonRequest>();
        request.PersonType = PersonType.LegalPerson;
        _mockRepository.Setup(m => m.LegalCustomerExists(request.Cnpj!)).ReturnsAsync(false);
        _mockKeycloakAdminService.Setup(k => k.CreateUserAsync(It.IsAny<UserKeycloak>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("company-id-123"); // Simula criação bem-sucedida

        // Act
        await _service.ExecuteAsync(request, CancellationToken.None);

        // Assert
        _mockOutputPort.Verify(o => o.LegalPersonAlreadyExists(), Times.Never);
        _mockOutputPort.Verify(o => o.LegalPersonCreated(It.IsAny<LegalPerson>()), Times.Once);
        _mockRepository.Verify(
            r => r.CreatePerson(It.Is<LegalPerson>(p => p.KeycloakId == "company-id-123"),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreatePerson_When_PersonTypeNotSupported_Should_CallPersonTypeNotSupported()
    {
        // Arrange
        var request = _fixture.Create<CreatePersonRequest>();
        request.PersonType = (PersonType)999; // Invalid PersonType

        // Act
        await _service.ExecuteAsync(request, CancellationToken.None);

        // Assert
        _mockOutputPort.Verify(o => o.PersonTypeNotSupported(), Times.Once);
        _mockOutputPort.Verify(o => o.NaturalPersonCreated(It.IsAny<NaturalPerson>()), Times.Never);
        _mockOutputPort.Verify(o => o.LegalPersonCreated(It.IsAny<LegalPerson>()), Times.Never);
        _mockRepository.Verify(r => r.CreatePerson(It.IsAny<NaturalPerson>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockRepository.Verify(r => r.CreatePerson(It.IsAny<LegalPerson>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}