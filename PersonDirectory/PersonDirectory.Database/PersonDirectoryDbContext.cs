using Microsoft.EntityFrameworkCore;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.DbContext;

namespace PersonDirectory.Database;

public sealed class PersonDirectoryDbContext : DbContext, IPersonDirectoryDbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<RelatedPersons> RelatedPeople { get; set; }

    // add-migration InitialCreate
    // update-database

    public PersonDirectoryDbContext(DbContextOptions<PersonDirectoryDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasOne(p => p.City)
            .WithMany()
            .HasForeignKey(p => p.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PhoneNumber>()
            .HasOne(pn => pn.Person)
            .WithMany(p => p.PhoneNumbers)
            .HasForeignKey(pn => pn.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RelatedPersons>()
            .HasOne(rp => rp.Person)
            .WithMany(p => p.RelatedPeople)
            .HasForeignKey(rp => rp.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}