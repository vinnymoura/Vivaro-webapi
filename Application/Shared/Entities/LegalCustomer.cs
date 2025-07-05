using System.ComponentModel.DataAnnotations;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;

namespace Application.Shared.Entities;

public class LegalPerson() : Person
{
    [Required] [MaxLength(14)] public string Cnpj { get; init; } = null!;

    [Required] [MaxLength(100)] public string CompanyName { get; init; } = null!;
    
    [MaxLength(50)] public string? TradeName { get; init; }

    [MaxLength(36)] public string? StateRegistration { get; init; }

    public LegalPerson(CreatePersonRequest request, string keycloakId) : this()
    {
        CompanyName = request.CompanyName!;
        Cnpj = request.Cnpj!;
        TradeName = request.TradeName;
        StateRegistration = request.StateRegistration;
        base.Addresses = request.Address is not null
            ? new List<Address> { new Address(request.Address) }
            : new List<Address>();
        base.PhoneNumber = request.PhoneNumber;
        base.Email = request.Email;
        base.KeycloakId = keycloakId;
        base.PersonType = request.PersonType;
    }
}