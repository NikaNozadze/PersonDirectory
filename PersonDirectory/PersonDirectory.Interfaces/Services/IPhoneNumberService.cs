using PersonDirectory.DTO;

namespace PersonDirectory.Interfaces.Services
{
    public interface IPhoneNumberService
    {
        Task<PhoneNumber> GetPhoneNumber(int phoneNumberId);
        Task<IEnumerable<PhoneNumber>> GetPersonsAllPhoneNumbers(int personId);
        Task<IEnumerable<PhoneNumber>> GetAllPhoneNumbers();
        Task<PhoneNumber> CreatePhoneNumber(PhoneNumber phoneNumber);
        Task<PhoneNumber> UpdatePhoneNumber(PhoneNumber phoneNumber);
        Task DeletePhoneNumber(int phoneNumberId);
    }
}
