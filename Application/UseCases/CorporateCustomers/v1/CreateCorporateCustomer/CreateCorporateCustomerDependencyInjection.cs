using Application.Shared.Notifications;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Abstractions;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Models;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Services;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer;

public static class CreateCorporateCustomerDependencyInjection
{
    public static IServiceCollection AddCreateCorporateCustomerUseCase(this IServiceCollection services) => services
        .AddDependencies()
        .AddNotifications()
        .AddScoped<IValidator<CreateCorporateCustomerRequest>, CreateCorporateCustomerRequestValidator>()
        .AddScoped<ICreateCorporateCustomersUseCase, CreateCorporateCustomerUseCase>()
        .Decorate<ICreateCorporateCustomersUseCase, CreateCorporateCustomerValidationUseCase>();
}