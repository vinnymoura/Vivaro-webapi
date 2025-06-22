using Application.Shared.Context;
using Application.Shared.Entities;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Services.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Services.Repositories;

public class IndividualCustomerRepository(AppDbContext context) : IIndividualCustomerRepository
{
    public async Task<bool> IndividualCustomerExists(string cpf) =>
        await context.Users.OfType<IndividualCustomer>().AnyAsync(ic => ic.Cpf == cpf);

    public async Task CreateIndividualCustomer(IndividualCustomer individualCustomer,
        CancellationToken cancellationToken) =>
        await context.Users.AddAsync(individualCustomer, cancellationToken);
}