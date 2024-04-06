using PersonDirectory.DTO;

namespace PersonDirectory.Interfaces.Repositories
{
    public interface IRelatedPersonRepository : IRepositoryBase<RelatedPersons>
    {
        Task<IEnumerable<RelatedPersons>> GetAllPersonRelatedPersons(int personId);
    }
}
