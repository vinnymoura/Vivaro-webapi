using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Abstractions;

public interface ICreateIndividualCustomersUseCase
{
    void SetOutputPort(IOutputPort outputPort);
    Task ExecuteAsync(CreateIndividualCustomerRequest request, CancellationToken cancellationToken);
}