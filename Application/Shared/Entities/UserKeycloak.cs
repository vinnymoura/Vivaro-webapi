using Application.Shared.Enum;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;

namespace Application.Shared.Entities;

public class UserKeycloak()
{
    public string username { get; set; }
    public bool enabled { get; set; }
    public string email { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public object[] credentials { get; set; }

    public Dictionary<string, string> attributes { get; set; } = new Dictionary<string, string>();

    public UserKeycloak(CreatePersonRequest request) : this()
    {
        username = request.Email;
        email = request.Email;
        enabled = true;
        credentials =
        [
            new { type = "password", value = request.Password, temporary = false }
        ];

        attributes = new Dictionary<string, string>
        {
            { "personType", request.PersonType.ToString() }
        };

        if (request.PersonType == PersonType.NaturalPerson)
        {
            firstName = request.FirstName!;
            lastName = request.LastName!;
        }
        else if (request.PersonType == PersonType.LegalPerson)
        {
            firstName = request.CompanyName!;
            lastName = "Pessoa Jurídica";
        }
    }
}