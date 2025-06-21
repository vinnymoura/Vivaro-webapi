namespace Application.Shared.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    // Relacionamento com permissões
    public ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
}