using PersonDirectory.DTO;

namespace PersonDirectory.Interfaces.Repositories
{
    public interface IPhoneNumberRepository : IRepositoryBase<PhoneNumber>
    {
        Task<IEnumerable<PhoneNumber>> GetAllPersonPhoneNumbers(int personId);
    }
}
