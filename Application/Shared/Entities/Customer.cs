using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Shared.Entities;

public class Customer 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    public string KeycloakId { get; init; }
    public ICollection<Address> Addresses { get; protected init; } = new List<Address>();
    [MaxLength(15)] public string? PhoneNumber { get; init; }
}