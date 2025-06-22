using Application.Shared.Notifications;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Abstractions;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;
using AutoFixture;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace UnitTests.UseCases.IndividualCustomers.v1.Create;

public class CreateIndividualCustomerValidationUseCaseTests
{
    private readonly Mock<ICreateIndividualCustomersUseCase> _mockUseCase = new();
    private readonly Mock<IValidator<CreateIndividualCustomerRequest>> _mockValidator = new();
    private readonly Notification _notification = new();
    private readonly Mock<IOutputPort> _mockOutputPort = new();
    private readonly Fixture _fixture = new();
    private readonly CreateIndividualCustomerValidationUseCase _service;
    
    public CreateIndividualCustomerValidationUseCaseTests()
    {
        _service = new CreateIndividualCustomerValidationUseCase(_mockUseCase.Object, _mockValidator.Object,
            _notification);
        _service.SetOutputPort(_mockOutputPort.Object);
    }
    
    [Fact]
    public async Task ExecuteAsync_When_ValidatorReturnsErrors_ShouldCall_InvalidRequest()
    {
        var request = _fixture.Create<CreateIndividualCustomerRequest>();

        _mockValidator.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(_fixture.Create<ValidationResult>());

        await _service.ExecuteAsync(request, CancellationToken.None);

        _mockOutputPort.Verify(o => o.InvalidRequest(), Times.Once);
        _mockUseCase.Verify(m => m.ExecuteAsync(It.IsAny<CreateIndividualCustomerRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task ExecuteAsync_When_ValidatorReturnsSuccess_ShouldCall_UseCase()
    {
        var request = _fixture.Create<CreateIndividualCustomerRequest>();

        _mockValidator.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new ValidationResult() {Errors = [] });

        await _service.ExecuteAsync(request, CancellationToken.None);

        _mockOutputPort.Verify(o => o.InvalidRequest(), Times.Never);
        _mockUseCase.Verify(m => m.ExecuteAsync(request, CancellationToken.None),
            Times.Once);
    }
}