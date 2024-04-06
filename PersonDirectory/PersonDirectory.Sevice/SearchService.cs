using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Repositories;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model;

namespace PersonDirectory.Sevice
{
    public class SearchService : ISearchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<Person>> SearchPersonsAsync(string searchTerm)
        {
            var result = await _unitOfWork.SearchRepository.SearchPersonsAsync(searchTerm);

            return result;
        }

        public async Task<IEnumerable<City>> GetCityWithSTermAsync(string searchTerm)
        {
            return await _unitOfWork.SearchRepository.SearchCitiesAsync(searchTerm);
        }

        public async Task<IEnumerable<PhoneNumber>> GetPhoneNumbersWithSTermAsync(string searchTerm)
        {
            var phoneNumbers = await _unitOfWork.SearchRepository.SearchPhoneNumbersAsync(searchTerm);
            foreach (var phoneNumber in phoneNumbers)
            {
                phoneNumber.Person = await _unitOfWork.PersonRepository.GetAsync(phoneNumber.PersonId);
            }

            return phoneNumbers;
        }

        public async Task<IEnumerable<Person>> DetailedSearchPersonsAsync(SearchParametersModel parameters)
        {
            return await _unitOfWork.SearchRepository.DetailedSearchPersonsAsync(parameters);
        }
    }
}
