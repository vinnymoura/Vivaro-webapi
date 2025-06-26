using Application.Shared.Entities;
using Application.Shared.Services.Abstractions;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Abstractions;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Models;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Services.Repositories.Abstractions;

namespace Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer;

public class CreateCorporateCustomerUseCase(ICorporateCustomerRepository repository, IUnitOfWork unitOfWork)
    : ICreateCorporateCustomersUseCase
{
    private IOutputPort? _outputPort;

    public void SetOutputPort(IOutputPort outputPort) => _outputPort = outputPort;

    public async Task ExecuteAsync(CreateCorporateCustomerRequest request, CancellationToken cancellationToken)
    {
        if (_outputPort == null)
        {
            throw new InvalidOperationException("Output port not set");
        }

        var corporateCustomerExists = await repository.CorporateCustomerExists(request.Cnpj);
        if (corporateCustomerExists)
        {
            _outputPort.CorporateCustomerAlreadyExists();
            return;
        }

        var corporateCustomer = await SaveCorporateCustomerAsync(request, cancellationToken);

        _outputPort.CorporateCustomerCreated(corporateCustomer);
    }

    private async Task<CorporateCustomer> SaveCorporateCustomerAsync(CreateCorporateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var corporateCustomer = new CorporateCustomer(request);
        await repository.CreateCorporateCustomer(corporateCustomer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return corporateCustomer;
    }
}