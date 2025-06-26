using Application.Shared.Entities;

namespace Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Abstractions;

public interface IOutputPort
{
    void InvalidRequest();
    void CorporateCustomerAlreadyExists();
    void CorporateCustomerCreated(CorporateCustomer corporateCustomer);
}