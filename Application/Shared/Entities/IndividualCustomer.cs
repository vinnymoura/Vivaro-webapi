using System.ComponentModel.DataAnnotations;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;

namespace Application.Shared.Entities;

public class IndividualCustomer() : Customer
{
    [MaxLength(100)] [Required] public string Name { get; init; }
    [MaxLength(11)] [Required] public string Cpf { get; init; }
    public DateTime? BirthDate { get; init; }

    public IndividualCustomer(CreateIndividualCustomerRequest request) : this()
    {
        Name = request.Name;
        Cpf = request.Cpf;
        BirthDate = request.BirthDate;
        base.Addresses = request.Address is not null
            ? new List<Address> { new Address(request.Address) }
            : new List<Address>();
        base.CustomerStatus = Enum.EnumCustomerStatus.Inactive;
        base.PhoneNumber = request.PhoneNumber;
        base.Login = new Login(request.Login);
    }
}