using Application.UseCases.PersonUseCase.v1.CreatePerson.Services.Repositories;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Services.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.PersonUseCase.v1.CreatePerson.Services;

public static class PersonServicesDependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services) =>
        services.AddScoped<IPersonRepository, PersonRepository>();
}