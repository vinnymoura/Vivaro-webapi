using Application.UseCases.PersonUseCase.v1.CreatePerson;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Abstractions;
using FluentAssertions;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Http;
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
        services.AddSingleton<IConfiguration>(configuration);
        services.AddHttpClient();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.Configure<KeycloakProtectionClientOptions>(_ => { });

        services
            .ConfigureDataBase(configuration)
            .AddCreatePersonUseCase();

        var provider = services.BuildServiceProvider();

        var useCase = provider.GetService<ICreatePersonUseCase>();

        useCase.Should().NotBeNull();
    }
}