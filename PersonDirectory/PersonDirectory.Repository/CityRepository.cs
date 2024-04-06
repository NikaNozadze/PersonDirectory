using PersonDirectory.DTO;
using PersonDirectory.Interfaces.DbContext;
using PersonDirectory.Interfaces.Repositories;

namespace PersonDirectory.Repository
{
    internal sealed class CityRepository : RepositoryBase<City>, ICityRepository
    {
        internal CityRepository(IPersonDirectoryDbContext context) : base(context)
        {

        }
    }
}
