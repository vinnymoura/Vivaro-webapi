using Application.Shared.Context;
using Application.Shared.Entities;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Services.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Services.Repositories;

public class CorporateCustomerRepository(AppDbContext context) : ICorporateCustomerRepository
{
    public Task<bool> CorporateCustomerExists(string cnpj) =>
        context.Users.OfType<CorporateCustomer>().AnyAsync(cc => cc.Cnpj == cnpj);

    public async Task CreateCorporateCustomer(CorporateCustomer corporateCustomer,
        CancellationToken cancellationToken) =>
        await context.Users.AddAsync(corporateCustomer, cancellationToken);
}