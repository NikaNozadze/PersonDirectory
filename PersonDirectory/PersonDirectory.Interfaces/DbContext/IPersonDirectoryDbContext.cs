using Microsoft.EntityFrameworkCore;
using PersonDirectory.DTO;

namespace PersonDirectory.Interfaces.DbContext
{
    public interface IPersonDirectoryDbContext : IDbContext
    {
        DbSet<City> Cities { get; set; }
        DbSet<Person> People { get; set; }
        DbSet<PhoneNumber> PhoneNumbers { get; set; }
        DbSet<RelatedPersons> RelatedPeople { get; set; }

        int SaveChanges();
    }
}