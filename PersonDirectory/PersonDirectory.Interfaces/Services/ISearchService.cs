using PersonDirectory.DTO;
using PersonDirectory.Model;

namespace PersonDirectory.Interfaces.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<Person>> SearchPersonsAsync(string searchTerm);
        Task<IEnumerable<City>> GetCityWithSTermAsync(string searchTerm);
        Task<IEnumerable<PhoneNumber>> GetPhoneNumbersWithSTermAsync(string searchTerm);
        Task<IEnumerable<Person>> DetailedSearchPersonsAsync(SearchParametersModel parameters);
    }
}
