using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Shared.Models.Request;

namespace Application.Shared.Entities;

public class Login()
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    [Required] public string Access { get; set; }

    [Required] public string Password { get; set; }

    public Login(LoginRequest request) : this()
    {
        Access = request.Access;
        Password = request.Password;
    }
}