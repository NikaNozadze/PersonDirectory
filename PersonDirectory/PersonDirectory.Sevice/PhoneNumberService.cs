using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Repositories;
using PersonDirectory.Interfaces.Services;

namespace PersonDirectory.Service
{
    public sealed class PhoneNumberService : IPhoneNumberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhoneNumberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<PhoneNumber> GetPhoneNumber(int phoneNumberId)
        {
            return await _unitOfWork.PhoneNumberRepository.GetAsync(phoneNumberId);
        }

        public async Task<IEnumerable<PhoneNumber>> GetPersonsAllPhoneNumbers(int personId)
        {
            return await _unitOfWork.PhoneNumberRepository.GetAllPersonPhoneNumbers(personId);
        }

        public async Task<IEnumerable<PhoneNumber>> GetAllPhoneNumbers()
        {
            return await _unitOfWork.PhoneNumberRepository.GetAllAsync();
        }

        public async Task<PhoneNumber> CreatePhoneNumber(PhoneNumber phoneNumber)
        {
            _unitOfWork.PhoneNumberRepository.Insert(phoneNumber);
            await _unitOfWork.SaveChangesAsync();
            return phoneNumber;
        }

        public async Task<PhoneNumber> UpdatePhoneNumber(PhoneNumber phoneNumber)
        {
            _unitOfWork.PhoneNumberRepository.Update(phoneNumber);
            await _unitOfWork.SaveChangesAsync();
            return phoneNumber;
        }

        public async Task DeletePhoneNumber(int phoneNumberId)
        {
            var phoneNumber = await _unitOfWork.PhoneNumberRepository.GetAsync(phoneNumberId);
            if (phoneNumber != null)
            {
                _unitOfWork.PhoneNumberRepository.Delete(phoneNumber);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
