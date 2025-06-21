using Application.Shared.Entities;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Services.Repositories.Abstractions;

public interface IIndividualCustomerRepository
{
    bool IndividualCustomerExists(string cpf);
    Task CreateIndividualCustomer(IndividualCustomer individualCustomer, CancellationToken cancellationToken);
}