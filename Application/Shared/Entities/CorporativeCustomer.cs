using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Entities;

public class CorporateCustomer() : Customer
{
    [Required] [MaxLength(14)] public string Cnpj { get; init; } 
    [Required] public string CompanyName { get; init; } = null!;
    public string? TradeName { get; init; }
    public string? StateRegistration { get; init; }
}