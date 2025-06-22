using Application.Shared.Entities;
using Application.Shared.Services.Abstractions;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Abstractions;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Services.Repositories.Abstractions;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer;

public class CreateIndividualCustomerUseCase(IIndividualCustomerRepository repository, IUnitOfWork unitOfWork) : ICreateIndividualCustomersUseCase
{
    private IOutputPort? _outputPort;
    
    public void SetOutputPort(IOutputPort outputPort) => _outputPort = outputPort;

    public async Task ExecuteAsync(CreateIndividualCustomerRequest request, CancellationToken cancellationToken)
    {
        if (_outputPort == null)
        {
            throw new InvalidOperationException("Output port not set");
        }

        var individualCustomerExists = await repository.IndividualCustomerExists(request.Cpf);
        if (individualCustomerExists)
        {
            _outputPort.IndividualCustomerAlreadyExists();
            return;
        }

        var individualCustomer = await SaveIndividualCustomerAsync(request, cancellationToken);

        _outputPort.IndividualCustomerCreated(individualCustomer);
    }

    private async Task<IndividualCustomer> SaveIndividualCustomerAsync(CreateIndividualCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var individualCustomer = new IndividualCustomer(request);
        await repository.CreateIndividualCustomer(individualCustomer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return individualCustomer;
    }
}