using Application.Shared.Notifications;
using Application.Shared.Services.CpfValidator;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Abstractions;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Services;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer;

public static class CreateIndividualCustomerDependencyInjection
{
    public static IServiceCollection AddCreateIndividualCustomerUseCase(this IServiceCollection services) => services
        .AddDependencies()
        .AddNotifications()
        .AddScoped<IValidator<CreateIndividualCustomerRequest>, CreateIndividualCustomerRequestValidator>()
        .AddScoped<ICreateIndividualCustomersUseCase, CreateIndividualCustomerUseCase>()
        .Decorate<ICreateIndividualCustomersUseCase, CreateIndividualCustomerValidationUseCase>();
    
}