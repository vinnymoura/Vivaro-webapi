using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Services.Repositories;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Services.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Services;

public static class IndividualCustomerServicesDependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services) =>
        services.AddScoped<IIndividualCustomerRepository, IndividualCustomerRepository>();
}