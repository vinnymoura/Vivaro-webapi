using Application.Shared.Notifications;
using Application.Shared.Services.Keycloak;
using Application.Shared.Services.Keycloak.Abstractions;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Abstractions;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Services;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.PersonUseCase.v1.CreatePerson;

public static class CreatePersonDependencyInjection
{
    public static IServiceCollection AddCreatePersonUseCase(this IServiceCollection services) => services
        .AddDependencies()
        .AddNotifications()
        .AddScoped<IKeycloakAdminService, KeycloakAdminService>()
        .AddScoped<IValidator<CreatePersonRequest>, CreatePersonRequestValidator>()
        .AddScoped<ICreatePersonUseCase, CreatePersonUseCase>()
        .Decorate<ICreatePersonUseCase, CreatePersonValidationUseCase>();
    
}