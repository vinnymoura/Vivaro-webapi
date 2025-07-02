using Application.Shared.Models.Request;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;

public class CreateIndividualCustomerRequest
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; init; } = null!;
    public AddressRequest? Address { get; init; }
    public string Cpf { get; init; } = null!;
    public DateTime? BirthDate { get; init; }

   
}