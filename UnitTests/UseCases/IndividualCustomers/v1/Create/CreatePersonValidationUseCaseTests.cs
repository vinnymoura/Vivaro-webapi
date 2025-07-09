using Application.Shared.Notifications;
using Application.UseCases.PersonUseCase.v1.CreatePerson;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Abstractions;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;
using AutoFixture;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace UnitTests.UseCases.IndividualCustomers.v1.Create;

public class CreatePersonValidationUseCaseTests
{
    private readonly Mock<ICreatePersonUseCase> _mockUseCase = new();
    private readonly Mock<IValidator<CreatePersonRequest>> _mockValidator = new();
    private readonly Notification _notification = new();
    private readonly Mock<IOutputPort> _mockOutputPort = new();
    private readonly Fixture _fixture = new();
    private readonly CreatePersonValidationUseCase _service;
    
    public CreatePersonValidationUseCaseTests()
    {
        _service = new CreatePersonValidationUseCase(_mockUseCase.Object, _mockValidator.Object,
            _notification);
        _service.SetOutputPort(_mockOutputPort.Object);
    }
    
    [Fact]
    public async Task ExecuteAsync_When_ValidatorReturnsErrors_ShouldCall_InvalidRequest()
    {
        var request = _fixture.Create<CreatePersonRequest>();

        _mockValidator.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(_fixture.Create<ValidationResult>());

        await _service.ExecuteAsync(request, CancellationToken.None);

        _mockOutputPort.Verify(o => o.InvalidRequest(), Times.Once);
        _mockUseCase.Verify(m => m.ExecuteAsync(It.IsAny<CreatePersonRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task ExecuteAsync_When_ValidatorReturnsSuccess_ShouldCall_UseCase()
    {
        var request = _fixture.Create<CreatePersonRequest>();

        _mockValidator.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new ValidationResult() {Errors = [] });

        await _service.ExecuteAsync(request, CancellationToken.None);

        _mockOutputPort.Verify(o => o.InvalidRequest(), Times.Never);
        _mockUseCase.Verify(m => m.ExecuteAsync(request, CancellationToken.None),
            Times.Once);
    }
}