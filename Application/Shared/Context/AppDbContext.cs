using Application.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Shared.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<Login> Login { get; set; }
    public required DbSet<Address> Addresses { get; set; }
    public required DbSet<AdminUserRole> AdminUserRoles { get; set; }
    public required DbSet<RolePermission> RolePermissions { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<IndividualCustomer>("IndividualCustomer")
            .HasValue<CorporateCustomer>("CorporateCustomer")
            .HasValue<AdminUser>("AdminUser");

        _ = modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Administrador", Description = "Acesso total ao sistema" },
            new Role { Id = 2, Name = "Supervisor", Description = "Acesso intermediário" },
            new Role { Id = 3, Name = "Normal", Description = "Acesso básico" }
        );

        _ = modelBuilder.Entity<User>()
            .HasOne(u => u.Login)
            .WithOne()
            .HasForeignKey<Login>(l => l.UserId)
            .IsRequired();

        _ = modelBuilder.Entity<Login>()
            .HasKey(l => l.GuidId);

        _ = modelBuilder.Entity<Address>()
            .HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId)
            .IsRequired();
            
        // Configuração para AdminUserRole
        _ = modelBuilder.Entity<AdminUserRole>()
            .HasKey(ar => new { ar.AdminUserId, ar.RoleId });
            
        _ = modelBuilder.Entity<AdminUserRole>()
            .HasOne(ar => ar.AdminUser)
            .WithMany()
            .HasForeignKey(ar => ar.AdminUserId)
            .IsRequired();
            
        _ = modelBuilder.Entity<AdminUserRole>()
            .HasOne(ar => ar.Role)
            .WithMany()
            .HasForeignKey(ar => ar.RoleId)
            .IsRequired();
            
        // Configuração para RolePermission
        _ = modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });
            
        _ = modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany()
            .HasForeignKey(rp => rp.RoleId)
            .IsRequired();
            
        _ = modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId)
            .IsRequired();
    }
}