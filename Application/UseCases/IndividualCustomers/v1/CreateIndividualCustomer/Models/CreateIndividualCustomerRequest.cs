using Application.Shared.Models.Request;

namespace Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;

public class CreateIndividualCustomerRequest
{
    public string Name { get; set; } = null!;
    public LoginRequest Login { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public AddressRequest? Address { get; set; }
    public string Cpf { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
}