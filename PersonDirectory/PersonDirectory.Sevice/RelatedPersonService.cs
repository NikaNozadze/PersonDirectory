using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Repositories;
using PersonDirectory.Interfaces.Services;

namespace PersonDirectory.Service
{
    public sealed class RelatedPersonService : IRelatedPersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RelatedPersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<RelatedPersons> GetRelatedPerson(int relatedPersonId)
        {
            return await _unitOfWork.RelatedPersonRepository.GetAsync(relatedPersonId);
        }

        public async Task<IEnumerable<RelatedPersons>> GetPersonsAllRelatedPerson(int personId)
        {
            var relatedPersons = await _unitOfWork.RelatedPersonRepository.GetAllPersonRelatedPersons(personId);
            foreach (var relatedPerson in relatedPersons)
            {
                relatedPerson.Person = await _unitOfWork.PersonRepository.GetAsync(relatedPerson.PersonId);
                relatedPerson.RelatedPerson = await _unitOfWork.PersonRepository.GetAsync(relatedPerson.RelatedPersonId);
            }

            return relatedPersons;
        }

        public async Task<IEnumerable<RelatedPersons>> GetAllRelatedPersons()
        {
            return await _unitOfWork.RelatedPersonRepository.GetAllAsync();
        }

        public async Task<RelatedPersons> CreateRelatedPerson(RelatedPersons relatedPerson)
        {
            _unitOfWork.RelatedPersonRepository.Insert(relatedPerson);
            await _unitOfWork.SaveChangesAsync();
            return relatedPerson;
        }

        public async Task<RelatedPersons> UpdateRelatedPerson(RelatedPersons relatedPerson)
        {
            _unitOfWork.RelatedPersonRepository.Update(relatedPerson);
            await _unitOfWork.SaveChangesAsync();
            return relatedPerson;
        }

        public async Task DeleteRelatedPerson(int relatedPersonId)
        {
            var relatedPerson = await _unitOfWork.RelatedPersonRepository.GetAsync(relatedPersonId);
            if (relatedPerson != null)
            {
                _unitOfWork.RelatedPersonRepository.Delete(relatedPerson);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
