using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Shared.Models.Request;

namespace Application.Shared.Entities;

public class Login()
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid GuidId { get; init; }

    public Guid UserId { get; init; }

    [MaxLength(60)]
    [Required] public string Access { get; init; } = null!;

    [MaxLength(150)]
    [Required] public string Password { get; init; } = null!;

    public Login(LoginRequest request) : this()
    {
        Access = request.Access;
        Password = request.Password;
    }
}