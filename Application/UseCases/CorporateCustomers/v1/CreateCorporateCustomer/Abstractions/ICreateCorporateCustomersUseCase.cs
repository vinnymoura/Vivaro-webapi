using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Models;

namespace Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Abstractions;

public interface ICreateCorporateCustomersUseCase
{
    void SetOutputPort(IOutputPort outputPort);
    Task ExecuteAsync(CreateCorporateCustomerRequest request, CancellationToken cancellationToken);
}