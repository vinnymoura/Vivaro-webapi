using Application.Shared.Entities;

namespace Application.UseCases.PersonUseCase.v1.CreatePerson.Abstractions;

public interface IOutputPort
{
    void InvalidRequest();
    void NaturalPersonAlreadyExists();
    void LegalPersonAlreadyExists();
    void NaturalPersonCreated(NaturalPerson naturalPerson);
    void LegalPersonCreated(LegalPerson legalPerson);
    void KeycloakCreationFailed();
    void PersonTypeNotSupported();
}