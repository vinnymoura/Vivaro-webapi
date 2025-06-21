using System.ComponentModel.DataAnnotations;
using Application.Shared.Enum;

namespace Application.Shared.Entities;

public class Customer : User
{
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    [MaxLength(15)] public string? PhoneNumber { get; init; }
    public EnumCustomerStatus CustomerStatus { get; set; }
}