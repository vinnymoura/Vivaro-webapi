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
        if (!await ValidatePersonDoesNotExist(request))
            return;

        var keycloakId = await CreateKeycloakUser(request, cancellationToken);
        if (string.IsNullOrEmpty(keycloakId))
            return;

        var person = await CreateAndSavePerson(request, keycloakId, cancellationToken);
        NotifyPersonCreated(person);
    }

    private async Task<bool> ValidatePersonDoesNotExist(CreatePersonRequest request)
    {
        return request.PersonType switch
        {
            PersonType.NaturalPerson => await ValidateNaturalPersonDoesNotExist(request.Cpf!),
            PersonType.LegalPerson => await ValidateLegalPersonDoesNotExist(request.Cnpj!),
            _ => HandleUnsupportedPersonType()
        };
    }

    private async Task<bool> ValidateNaturalPersonDoesNotExist(string cpf)
    {
        var exists = await repository.IndividualCustomerExists(cpf);
        if (exists)
        {
            _outputPort!.NaturalPersonAlreadyExists();
            return false;
        }

        return true;
    }

    private async Task<bool> ValidateLegalPersonDoesNotExist(string cnpj)
    {
        var exists = await repository.LegalCustomerExists(cnpj);
        if (exists)
        {
            _outputPort!.LegalPersonAlreadyExists();
            return false;
        }

        return true;
    }

    private bool HandleUnsupportedPersonType()
    {
        _outputPort!.PersonTypeNotSupported();
        return false;
    }

    private async Task<string?> CreateKeycloakUser(CreatePersonRequest request, CancellationToken cancellationToken)
    {
        var userKeycloak = new UserKeycloak(request);
        var keycloakId = await keycloakService.CreateUserAsync(userKeycloak, cancellationToken);

        if (!string.IsNullOrEmpty(keycloakId)) return keycloakId;

        _outputPort!.KeycloakCreationFailed();
        return null;
    }

    private async Task<Person> CreateAndSavePerson(CreatePersonRequest request, string keycloakId,
        CancellationToken cancellationToken)
    {
        var person = CreatePersonEntity(request, keycloakId);
        await repository.CreatePerson(person, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return person;
    }

    private static Person CreatePersonEntity(CreatePersonRequest request, string keycloakId)
    {
        return request.PersonType switch
        {
            PersonType.NaturalPerson => new NaturalPerson(request, keycloakId),
            PersonType.LegalPerson => new LegalPerson(request, keycloakId),
            _ => throw new InvalidOperationException($"Tipo de pessoa não suportado: {request.PersonType}")
        };
    }

    private void NotifyPersonCreated(Person person)
    {
        switch (person)
        {
            case NaturalPerson naturalPerson:
                _outputPort!.NaturalPersonCreated(naturalPerson);
                break;
            case LegalPerson legalPerson:
                _outputPort!.LegalPersonCreated(legalPerson);
                break;
        }
    }
}