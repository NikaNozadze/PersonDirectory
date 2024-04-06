using PersonDirectory.DTO;
using PersonDirectory.Interfaces.DbContext;
using PersonDirectory.Interfaces.Repositories;

namespace PersonDirectory.Repository
{
    internal sealed class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        internal PersonRepository(IPersonDirectoryDbContext context) : base(context)
        {

        }
    }
}
