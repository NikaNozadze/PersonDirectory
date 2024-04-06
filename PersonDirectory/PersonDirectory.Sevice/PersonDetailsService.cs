using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Repositories;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model;

namespace PersonDirectory.Service
{
    public class PersonDetailsService : IPersonDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonDetailsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Person> GetPersonDetailsAsync(int personId)
        {
            var person = await _unitOfWork.PersonRepository.GetAsync(personId);
            if (person == null) throw new ArgumentException($"A person with this ID was not found.");
            var personDetails = await GetPersonDetailsFromDatabase(new List<Person> { person });

            return personDetails?.FirstOrDefault() ?? throw new ArgumentException($"A person with this ID was not found.");
        }

        public async Task<IEnumerable<Person>> GetPersonDetailsWithSTermAsync(string searchTerm)
        {
            var persons = await _unitOfWork.SearchRepository.SearchPersonsAsync(searchTerm);

            return await GetPersonDetailsFromDatabase(persons);
        }

        public async Task<IEnumerable<Person>> GetPersonDetailsWithDetailedSearchAsync(SearchParametersModel parameters)
        {
            var persons = await _unitOfWork.SearchRepository.DetailedSearchPersonsAsync(parameters);

            return await GetPersonDetailsFromDatabase(persons);
        }

        public byte[] GetImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                throw new FileNotFoundException($"Image file not found.");
            }

            byte[] fileContent = File.ReadAllBytes(imagePath);

            return fileContent;
        }

        private async Task<IEnumerable<Person>> GetPersonDetailsFromDatabase(IEnumerable<Person> persons)
        {
            foreach (var person in persons)
            {
                person.City = await _unitOfWork.CityRepository.GetAsync(person.CityId);
                person.PhoneNumbers = (await _unitOfWork.PhoneNumberRepository.GetAllAsync())
                                                .Where(pn => pn.PersonId == person.Id)
                                                .ToList();
                person.RelatedPeople = (await _unitOfWork.RelatedPersonRepository.GetAllAsync())
                                                .Where(rp => rp.PersonId == person.Id)
                                                .ToList();
                foreach (var relatedPerson in person.RelatedPeople)
                {
                    relatedPerson.RelatedPerson = await _unitOfWork.PersonRepository.GetAsync(relatedPerson.RelatedPersonId);
                }
            }

            return persons;
        }
    }
}
