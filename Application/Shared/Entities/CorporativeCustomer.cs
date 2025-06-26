using System.ComponentModel.DataAnnotations;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Models;

namespace Application.Shared.Entities;

public class CorporateCustomer() : Customer
{
    [Required] [MaxLength(14)] public string Cnpj { get; init; } = null!;
    [Required] public string CompanyName { get; init; } = null!;
    public string? TradeName { get; init; }
    public string? StateRegistration { get; init; }

    public CorporateCustomer(CreateCorporateCustomerRequest request) : this()
    {
        CompanyName = request.CompanyName;
        Cnpj = request.Cnpj;
        TradeName = request.TradeName;
        base.Addresses = request.Address is not null
            ? new List<Address> { new Address(request.Address) }
            : new List<Address>();
        base.PhoneNumber = request.PhoneNumber;
        base.CustomerStatus = Enum.EnumCustomerStatus.Inactive;
        base.Login = new Login(request.Login);
    }
}