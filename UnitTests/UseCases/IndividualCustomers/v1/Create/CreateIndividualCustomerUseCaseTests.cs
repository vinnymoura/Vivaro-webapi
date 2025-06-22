using Application.Shared.Entities;
using Application.Shared.Services.Abstractions;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Abstractions;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Services.Repositories.Abstractions;
using AutoFixture;
using Moq;
using Xunit;

namespace UnitTests.UseCases.IndividualCustomers.v1.Create;

public class CreateIndividualCustomerUseCaseTests
{
    private readonly Mock<IIndividualCustomerRepository> _mockRepository = new();
    private readonly Mock<IOutputPort> _mockOutputPort = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Fixture _fixture = new();
    private readonly CreateIndividualCustomerUseCase _service;

    public CreateIndividualCustomerUseCaseTests()
    {
        _service = new CreateIndividualCustomerUseCase(_mockRepository.Object, _mockUnitOfWork.Object);
        _service.SetOutputPort(_mockOutputPort.Object);
    }

    [Fact]
    public async Task CreateIndividualCustomer_When_CustomerExists_Should_CallCustomerAlreadyExists()
    {
        var request = _fixture.Create<CreateIndividualCustomerRequest>();
        _mockRepository.Setup(m => m.IndividualCustomerExists(request.Cpf)).ReturnsAsync(true);

        await _service.ExecuteAsync(request, CancellationToken.None);

        _mockOutputPort.Verify(o => o.IndividualCustomerAlreadyExists(), Times.Once);
        _mockOutputPort.Verify(o => o.IndividualCustomerCreated(It.IsAny<IndividualCustomer>()), Times.Never);
        _mockRepository.Verify(
            r => r.CreateIndividualCustomer(It.IsAny<IndividualCustomer>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateIndividualCustomer_When_CustomerDoesNotExist_Should_CreateCustomer()
    {
        var request = _fixture.Create<CreateIndividualCustomerRequest>();
        _mockRepository.Setup(m => m.IndividualCustomerExists(request.Cpf)).ReturnsAsync(false);

        await _service.ExecuteAsync(request, CancellationToken.None);

        _mockOutputPort.Verify(o => o.IndividualCustomerAlreadyExists(), Times.Never);
        _mockOutputPort.Verify(o => o.IndividualCustomerCreated(It.IsAny<IndividualCustomer>()), Times.Once);
        _mockRepository.Verify(
            r => r.CreateIndividualCustomer(It.IsAny<IndividualCustomer>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}