using Application.Shared.Entities;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Abstractions;

public interface IOutputPort
{
    void InvalidRequest();
    void IndividualCustomerAlreadyExists();
    void IndividualCustomerCreated(IndividualCustomer individualCustomer);
    void KeycloakCreationFailed();
}