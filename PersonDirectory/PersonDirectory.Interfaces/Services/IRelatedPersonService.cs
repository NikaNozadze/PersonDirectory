using PersonDirectory.DTO;

namespace PersonDirectory.Interfaces.Services
{
    public interface IRelatedPersonService
    {
        Task<RelatedPersons> GetRelatedPerson(int relatedPersonId);
        Task<IEnumerable<RelatedPersons>> GetPersonsAllRelatedPerson(int personId);
        Task<IEnumerable<RelatedPersons>> GetAllRelatedPersons();
        Task<RelatedPersons> CreateRelatedPerson(RelatedPersons relatedPerson);
        Task<RelatedPersons> UpdateRelatedPerson(RelatedPersons relatedPerson);
        Task DeleteRelatedPerson(int relatedPersonId);
    }
}
