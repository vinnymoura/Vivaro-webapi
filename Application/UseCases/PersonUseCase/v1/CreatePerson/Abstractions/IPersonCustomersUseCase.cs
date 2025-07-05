using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;

namespace Application.UseCases.PersonUseCase.v1.CreatePerson.Abstractions;

public interface IPersonCustomersUseCase
{
    void SetOutputPort(IOutputPort outputPort);
    Task ExecuteAsync(CreatePersonRequest request, CancellationToken cancellationToken);
}