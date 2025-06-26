using Application.Shared.Notifications;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Abstractions;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Models;
using FluentValidation;

namespace Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer;

public class CreateCorporateCustomerValidationUseCase(
    ICreateCorporateCustomersUseCase useCase,
    IValidator<CreateCorporateCustomerRequest> validator,
    Notification notification) : ICreateCorporateCustomersUseCase
{
    private IOutputPort? _outputPort;


    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        useCase.SetOutputPort(outputPort);
    }

    public async Task ExecuteAsync(CreateCorporateCustomerRequest request, CancellationToken cancellationToken)
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