using PersonDirectory.DTO;
using PersonDirectory.Model;

namespace PersonDirectory.Interfaces.Services
{
    public interface IPersonDetailsService
    {
        Task<Person> GetPersonDetailsAsync(int personId);
        Task<IEnumerable<Person>> GetPersonDetailsWithSTermAsync(string searchTerm);
        Task<IEnumerable<Person>> GetPersonDetailsWithDetailedSearchAsync(SearchParametersModel parameters);
    }
}