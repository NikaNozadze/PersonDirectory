using PersonDirectory.DTO;

namespace PersonDirectory.Interfaces.Services
{
    public interface ICityService
    {
        Task<City> GetCity(int cityId);
        Task<IEnumerable<City>> GetAllCities();
        Task<City> CreateCity(City city);
        Task<City> UpdateCity(City city);
        Task DeleteCity(int cityId);
    }
}
