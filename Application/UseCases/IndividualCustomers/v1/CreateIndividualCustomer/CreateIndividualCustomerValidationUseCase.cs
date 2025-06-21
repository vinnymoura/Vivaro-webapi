using Application.Shared.Notifications;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Abstractions;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;
using FluentValidation;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer;

public class CreateIndividualCustomerValidationUseCase(
    ICreateIndividualCustomersUseCase useCase,
    IValidator<CreateIndividualCustomerRequest> validator,
    Notification notification) : ICreateIndividualCustomersUseCase
{
    
    private IOutputPort? _outputPort;
    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        useCase.SetOutputPort(outputPort);
    }

    public async Task ExecuteAsync(CreateIndividualCustomerRequest request, CancellationToken cancellationToken)
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