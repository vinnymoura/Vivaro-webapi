using Application.Shared.Models.Request;

namespace Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Models;

public class CreateCorporateCustomerRequest
{
    public string CompanyName { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public AddressRequest? Address { get; init; }
    public string? TradeName { get; init; }
    public string? StateRegistration { get; init; }
    public string Cnpj { get; init; } = null!;
}