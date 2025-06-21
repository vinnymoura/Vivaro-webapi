namespace Application.Shared.Entities;

public class AdminUserRole
{
    public Guid AdminUserId { get; set; }
    public AdminUser AdminUser { get; set; } = null!;
    
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}