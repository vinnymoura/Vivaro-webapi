using System.ComponentModel.DataAnnotations;
using Application.Shared.Enum;
using Application.Shared.Models.Request;

namespace Application.UseCases.PersonUseCase.v1.CreatePerson.Models;

public class CreatePersonRequest
{
    [Required] public PersonType PersonType { get; set; }

    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    [Required] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public AddressRequest? Address { get; init; }
    public string? Cpf { get; init; }
    public DateTime? BirthDate { get; init; }

    public string? CompanyName { get; set; }
    public string? TradeName { get; set; }
    public string? StateRegistration { get; set; }
    public string? Cnpj { get; set; }
}