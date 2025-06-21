namespace Application.Shared.Entities;

public class AdminUser() : User
{
    public DateTime HireDate { get; set; } // The date when the admin user was hired
    public ICollection<AdminUserRole> Roles { get; set; } = new List<AdminUserRole>();
}