using Application.UseCases.PersonUseCase.v1.CreatePerson;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vivaro_webapi.Modules;
using Xunit;

namespace UnitTests.UseCases.IndividualCustomers.v1.Create;

public class CreatePersonDependencyInjectionTests
{
    [Fact]
    public void Should_Get_CreateIndividualCustomerUseCase()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json").Build();
        var services = new ServiceCollection();

        services
            .ConfigureDataBase(configuration)
            .AddCreatePersonUseCase();

        var provider = services.BuildServiceProvider();

        var useCase = provider.GetService<IPersonCustomersUseCase>();

        useCase.Should().NotBeNull();
    }
}