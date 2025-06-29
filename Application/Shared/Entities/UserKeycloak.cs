using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;

namespace Application.Shared.Entities;

public class UserKeycloak()
{
    public string username { get; set; }
    public bool enabled { get; set; }
    public string email { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public object[] credentials { get; set; }

    public UserKeycloak(CreateIndividualCustomerRequest request) : this()
    {
        username = request.Email;
        email = request.Email;
        firstName = request.FirstName;
        lastName = request.LastName;
        enabled = true;
        credentials =
        [
            new { type = "password", value = request.Password, temporary = false }
        ];
    }
}