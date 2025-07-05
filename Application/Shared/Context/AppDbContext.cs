using Application.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Shared.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public required DbSet<Address> Addresses { get; set; }
    public required DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Person>().ToTable("Persons");
        _ = modelBuilder.Entity<NaturalPerson>().ToTable("NaturalPersons");
        _ = modelBuilder.Entity<LegalPerson>().ToTable("LegalPersons");

        _ = modelBuilder.Entity<NaturalPerson>()
            .HasIndex(nc => nc.Cpf)
            .IsUnique();

        _ = modelBuilder.Entity<LegalPerson>()
            .HasIndex(lc => lc.Cnpj)
            .IsUnique();

        _ = modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithMany(p => p.Addresses) 
            .HasForeignKey(a => a.PersonId)
            .IsRequired();
    }
}