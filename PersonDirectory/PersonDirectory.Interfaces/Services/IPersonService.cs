using PersonDirectory.DTO;

namespace PersonDirectory.Interfaces.Services
{
    public interface IPersonService
    {
        Task<Person> GetPerson(int personId);
        Task<IEnumerable<Person>> GetAllPersons();
        Task<Person> CreatePerson(Person person);
        Task<Person> UpdatePerson(Person person);
        Task DeletePerson(Person person);
    }
}
