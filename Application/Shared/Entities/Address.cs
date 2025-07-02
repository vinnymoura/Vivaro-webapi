using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Shared.Models.Request;

namespace Application.Shared.Entities;

public class Address()
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [MaxLength(255)] public string RecipienteName { get; set; } = null!; // Name of the recipient, e.g., "John Doe", "Jane Smith"
    [MaxLength(255)] public string Street { get; set; } = null!; // e.g., "123 Main St", "456 Elm St"
    [MaxLength(50)] public string Number { get; set; } = null!; // e.g., "123", "456A"
    [MaxLength(100)] public string? Complement { get; set; }
    [MaxLength(100)] public string Neighborhood { get; set; } = null!;
    [MaxLength(100)] public string City { get; set; } = null!; // e.g., "New York", "São Paulo"
    [MaxLength(100)] public string State { get; set; } = null!; // e.g., "NY", "SP"
    [MaxLength(20)] public string ZipCode { get; set; } = null!; // e.g., "12345-678"
    [MaxLength(50)] public string Country { get; set; } = null!; // e.g., "USA", "Brazil"

    public Guid UserId { get; set; } // Foreign key to User entity
    [Required]
    public Customer User { get; set; } = null!;

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