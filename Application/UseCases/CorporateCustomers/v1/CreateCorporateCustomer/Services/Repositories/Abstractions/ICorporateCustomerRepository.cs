using Application.Shared.Entities;

namespace Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Services.Repositories.Abstractions;

public interface ICorporateCustomerRepository
{
    Task<bool> CorporateCustomerExists(string cnpj);    
    Task CreateCorporateCustomer(CorporateCustomer corporateCustomer, CancellationToken cancellationToken);
}