using PersonDirectory.DTO;
using PersonDirectory.Model;

namespace PersonDirectory.Interfaces.Repositories
{
    public interface ISearchRepository
    {
        Task<IEnumerable<Person>> SearchPersonsAsync(string searchTerm);
        Task<IEnumerable<Person>> DetailedSearchPersonsAsync(SearchParametersModel parameters);
        Task<IEnumerable<City>> SearchCitiesAsync(string searchTerm);
        Task<IEnumerable<PhoneNumber>> SearchPhoneNumbersAsync(string searchTerm);
    }
}