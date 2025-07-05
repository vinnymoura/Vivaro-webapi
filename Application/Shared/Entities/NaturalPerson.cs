using System.ComponentModel.DataAnnotations;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;

namespace Application.Shared.Entities;

public class NaturalPerson() : Person
{
    [MaxLength(50)] [Required] public string FirstName { get; init; } = null!;
    [MaxLength(50)] [Required] public string LastName { get; init; } = null!;
    [MaxLength(11)] [Required] public string Cpf { get; init; } = null!;
    public DateTime? BirthDate { get; init; }

    public NaturalPerson(CreatePersonRequest request, string keycloakId) : this()
    {
        FirstName = request.FirstName!;
        LastName = request.LastName!;
        Cpf = request.Cpf!;
        BirthDate = request.BirthDate;
        Email = request.Email;
        base.Addresses = request.Address is not null
            ? new List<Address> { new Address(request.Address) }
            : new List<Address>();
        base.PhoneNumber = request.PhoneNumber;
        base.KeycloakId = keycloakId;
        base.PersonType = request.PersonType;
    }
}