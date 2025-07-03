using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Shared.Enum;

namespace Application.Shared.Entities;

public abstract class Person()
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    public PersonType PersonType { get; init; }

    [MaxLength(50)] public string KeycloakId { get; init; } = null!;

    public ICollection<Address> Addresses { get; protected init; } = new List<Address>();
    [MaxLength(15)] public string? PhoneNumber { get; init; }

    [EmailAddress]
    [Required]
    [MaxLength(100)]
    public string Email { get; init; } = null!;

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public string Password { get; init; } = null!;
}