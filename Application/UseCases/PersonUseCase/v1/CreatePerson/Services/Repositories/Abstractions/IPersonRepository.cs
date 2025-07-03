using Application.Shared.Entities;

namespace Application.UseCases.PersonUseCase.v1.CreatePerson.Services.Repositories.Abstractions;

public interface IPersonRepository
{
    Task<bool> IndividualCustomerExists(string cpf);
    Task<bool> LegalCustomerExists(string requestCnpj);
    Task CreatePerson(Person person, CancellationToken cancellationToken);
}