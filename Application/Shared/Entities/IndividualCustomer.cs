using System.ComponentModel.DataAnnotations;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;

namespace Application.Shared.Entities;

public class IndividualCustomer() : Customer
{
    [MaxLength(50)] [Required] public string FirstName { get; init; } = null!;
    [MaxLength(50)] [Required] public string LastName { get; init; } = null!;
    [MaxLength(11)] [Required] public string Cpf { get; init; } = null!;

    [EmailAddress]
    [Required]
    [MaxLength(100)]
    public string Email { get; init; } = null!;

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public string Password { get; set; } = null!;

    public DateTime? BirthDate { get; init; }

    public IndividualCustomer(CreateIndividualCustomerRequest request, string keycloakId) : this()
    {
        FirstName = request.FirstName;
        LastName = request.LastName;
        Cpf = request.Cpf;
        BirthDate = request.BirthDate;
        Email = request.Email;
        Password = request.Password;
        base.Addresses = request.Address is not null
            ? new List<Address> { new Address(request.Address) }
            : new List<Address>();
        base.PhoneNumber = request.PhoneNumber;
        base.KeycloakId = keycloakId;
    }
}