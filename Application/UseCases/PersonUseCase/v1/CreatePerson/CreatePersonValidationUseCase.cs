using Application.Shared.Notifications;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Abstractions;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;
using FluentValidation;

namespace Application.UseCases.PersonUseCase.v1.CreatePerson;

public class CreatePersonValidationUseCase(
    ICreatePersonUseCase useCase,
    IValidator<CreatePersonRequest> validator,
    Notification notification) : ICreatePersonUseCase
{
    
    private IOutputPort? _outputPort;
    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        useCase.SetOutputPort(outputPort);
    }

    public async Task ExecuteAsync(CreatePersonRequest request, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(request, cancellationToken);

        notification.AddErrorMessages(result);

        if (notification.IsInvalid)
        {
            _outputPort!.InvalidRequest();
            return;
        }

        await useCase.ExecuteAsync(request, cancellationToken);
    }
}