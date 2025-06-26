using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Services.Repositories;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Services.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Services;

public static class CorporateCustomerServiceDependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services) =>
        services.AddScoped<ICorporateCustomerRepository, CorporateCustomerRepository>();
}