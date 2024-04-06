using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Repositories;
using PersonDirectory.Interfaces.Services;

namespace PersonDirectory.Service
{
    public sealed class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Person> GetPerson(int personId)
        {
            return await _unitOfWork.PersonRepository.GetAsync(personId);
        }

        public async Task<IEnumerable<Person>> GetAllPersons()
        {
            return await _unitOfWork.PersonRepository.GetAllAsync();
        }

        public async Task<Person> CreatePerson(Person person)
        {
            _unitOfWork.PersonRepository.Insert(person);
            await _unitOfWork.SaveChangesAsync();
            return person;
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            _unitOfWork.PersonRepository.Update(person);
            await _unitOfWork.SaveChangesAsync();
            return person;
        }

        public async Task DeletePerson(Person person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));

            _unitOfWork.PersonRepository.Delete(person);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
