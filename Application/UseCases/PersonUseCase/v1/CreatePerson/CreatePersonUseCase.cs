using Application.Shared.Entities;
using Application.Shared.Enum;
using Application.Shared.Services.Abstractions;
using Application.Shared.Services.Keycloak.Abstractions;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Abstractions;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Services.Repositories.Abstractions;

namespace Application.UseCases.PersonUseCase.v1.CreatePerson;

public class CreatePersonUseCase(
    IPersonRepository repository,
    IKeycloakAdminService keycloakService,
    IUnitOfWork unitOfWork) : IPersonCustomersUseCase
{
    private IOutputPort? _outputPort;

    public void SetOutputPort(IOutputPort outputPort) => _outputPort = outputPort;

    public async Task ExecuteAsync(CreatePersonRequest request, CancellationToken cancellationToken)
    {
        switch (request.PersonType)
        {
            case PersonType.NaturalPerson:
            {
                var individualCustomerExists = await repository.IndividualCustomerExists(request.Cpf!);
                if (individualCustomerExists)
                {
                    _outputPort!.NaturalPersonAlreadyExists();
                    return;
                }

                break;
            }
            case PersonType.LegalPerson:
            {
                var corporateCustomerExists = await repository.LegalCustomerExists(request.Cnpj!);
                if (corporateCustomerExists)
                {
                    _outputPort!.LegalPersonAlreadyExists();
                    return;
                }

                break;
            }
            default:
                _outputPort!.PersonTypeNotSupported();
                return;
        }

        var userKeycloak = new UserKeycloak(request);
        var keycloakId = await keycloakService.CreateUserAsync(userKeycloak, cancellationToken);

        if (string.IsNullOrEmpty(keycloakId))
        {
            _outputPort!.KeycloakCreationFailed();
            return;
        }

        var personToCreate = await SaveIndividualCustomerAsync(request, keycloakId, cancellationToken);

        switch (personToCreate)
        {
            case NaturalPerson naturalPerson:
                _outputPort!.NaturalPersonCreated(naturalPerson);
                break;
            case LegalPerson legalPerson:
                _outputPort!.LegalPersonCreated(legalPerson);
                break;
        }
    }

    private async Task<Person> SaveIndividualCustomerAsync(CreatePersonRequest request,
        string keycloakId,
        CancellationToken cancellationToken)
    {
        Person personToCreate;
        if (request.PersonType == PersonType.NaturalPerson)
        {
            personToCreate = new NaturalPerson(request, keycloakId);
        }
        else
        {
            personToCreate = new LegalPerson(request, keycloakId);
        }

        await repository.CreatePerson(personToCreate, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return personToCreate;
    }
}