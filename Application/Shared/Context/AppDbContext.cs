using Application.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Shared.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public required DbSet<Address> Addresses { get; set; }
    public required DbSet<Customer> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Customer>()
            .HasDiscriminator<string>("UserType")
            .HasValue<IndividualCustomer>("IndividualCustomer")
            .HasValue<CorporateCustomer>("CorporateCustomer");
            
        _ = modelBuilder.Entity<Address>()
            .HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId)
            .IsRequired();
    }
}