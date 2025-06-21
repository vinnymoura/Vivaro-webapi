using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Shared.Models.Request;

namespace Application.Shared.Entities;

public class Address()
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid GuidId { get; set; }
    
    [MaxLength(255)] public string RecipienteName { get; set; }
    [MaxLength(255)] public string Street { get; set; }
    [MaxLength(50)] public string Number { get; set; }
    [MaxLength(100)] public string? Complement { get; set; }
    [MaxLength(100)] public string Neighborhood { get; set; }
    [MaxLength(100)] public string City { get; set; }
    [MaxLength(100)] public string State { get; set; }
    [MaxLength(20)] public string ZipCode { get; set; } // e.g., "12345-678"
    [MaxLength(50)] public string Country { get; set; } // e.g., "USA", "Brazil"

    public Guid CustomerId { get; set; } // Foreign key to User entity
    [Required]
    public Customer Customer { get; set; } = null!;

    public Address(AddressRequest? address) : this()
    {
        RecipienteName = address!.RecipienteName;
        Street = address.Street;
        Number = address.Number;
        Complement = address.Complement;
        Neighborhood = address.Neighborhood;
        City = address.City;
        State = address.State;
        ZipCode = address.ZipCode;
        Country = address.Country;
    }
}