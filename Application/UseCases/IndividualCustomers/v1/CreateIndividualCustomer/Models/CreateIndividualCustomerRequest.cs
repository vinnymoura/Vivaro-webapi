using Application.Shared.Models.Request;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;

public class CreateIndividualCustomerRequest
{
    public string Name { get; init; } = null!;
    public LoginRequest Login { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public AddressRequest? Address { get; init; }
    public string Cpf { get; init; } = null!;
    public DateTime? BirthDate { get; init; }
}