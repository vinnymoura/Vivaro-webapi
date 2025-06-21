using Application.Shared.Context;
using Application.Shared.Entities;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Services.Repositories.Abstractions;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Services.Repositories;

public class IndividualCustomerRepository(AppDbContext context) : IIndividualCustomerRepository
{
    public bool IndividualCustomerExists(string cpf) =>
        context.Users.OfType<IndividualCustomer>().Any(ic => ic.Cpf == cpf);

    public async Task CreateIndividualCustomer(IndividualCustomer individualCustomer, CancellationToken cancellationToken) =>
        await context.Users.AddAsync(individualCustomer, cancellationToken);
}