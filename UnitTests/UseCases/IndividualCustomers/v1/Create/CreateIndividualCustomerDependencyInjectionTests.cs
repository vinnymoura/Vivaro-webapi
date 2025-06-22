using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vivaro_webapi.Modules;
using Xunit;

namespace UnitTests.UseCases.IndividualCustomers.v1.Create;

public class CreateIndividualCustomerDependencyInjectionTests
{
    [Fact]
    public void Should_Get_CreateIndividualCustomerUseCase()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json").Build();
        var services = new ServiceCollection();

        services
            .ConfigureDataBase(configuration)
            .AddCreateIndividualCustomerUseCase();

        var provider = services.BuildServiceProvider();

        var useCase = provider.GetService<ICreateIndividualCustomersUseCase>();

        useCase.Should().NotBeNull();
    }
}