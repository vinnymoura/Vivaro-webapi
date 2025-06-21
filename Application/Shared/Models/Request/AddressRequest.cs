namespace Application.Shared.Models.Request;

public class AddressRequest
{
    public string RecipienteName { get; set; } = null!; // e.g., "John Doe", "Jane Smith"
    public string Street { get; set; } = null!; // e.g., "123 Main St", "456 Elm St"
    public string Number { get; set; } = null!; // e.g., "123", "456"
    public string? Complement { get; set; }
    public string Neighborhood { get; set; } = null!; // e.g., "Downtown", "Uptown"
    public string City { get; set; } = null!; // e.g., "New York", "Los Angeles"
    public string State { get; set; } = null!; // e.g., "NY", "CA"
    public string ZipCode { get; set; } = null!; // e.g., "12345-678"
    public string Country { get; set; } = null!; // e.g., "USA", "Brazil"
}