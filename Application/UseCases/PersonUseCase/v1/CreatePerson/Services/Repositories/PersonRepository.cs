using Application.Shared.Context;
using Application.Shared.Entities;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Services.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.PersonUseCase.v1.CreatePerson.Services.Repositories;

public class PersonRepository(AppDbContext context) : IPersonRepository
{
    public async Task<bool> IndividualCustomerExists(string cpf) =>
        await context.Persons.OfType<NaturalPerson>().AnyAsync(ic => ic.Cpf == cpf);

    public async Task<bool> LegalCustomerExists(string cnpj) =>
        await context.Persons.OfType<LegalPerson>().AnyAsync(lc => lc.Cnpj == cnpj);

    public async Task CreatePerson(Person person, CancellationToken cancellationToken) =>
        await context.Persons.AddAsync(person, cancellationToken);
}