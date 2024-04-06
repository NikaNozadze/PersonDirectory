using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Repositories;
using PersonDirectory.Interfaces.Services;

namespace PersonDirectory.Service
{
    public sealed class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<City> GetCity(int cityId)
        {
            return await _unitOfWork.CityRepository.GetAsync(cityId);
        }

        public async Task<IEnumerable<City>> GetAllCities()
        {
            return await _unitOfWork.CityRepository.GetAllAsync();
        }

        public async Task<City> CreateCity(City city)
        {
            _unitOfWork.CityRepository.Insert(city);
            await _unitOfWork.SaveChangesAsync();
            return city;
        }

        public async Task<City> UpdateCity(City city)
        {
            _unitOfWork.CityRepository.Update(city);
            await _unitOfWork.SaveChangesAsync();
            return city;
        }

        public async Task DeleteCity(int cityId)
        {
            var city = await _unitOfWork.CityRepository.GetAsync(cityId);
            if (city != null)
            {
                _unitOfWork.CityRepository.Delete(city);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
