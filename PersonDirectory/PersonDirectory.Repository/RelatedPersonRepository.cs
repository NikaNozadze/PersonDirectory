using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.DbContext;
using PersonDirectory.Interfaces.Repositories;

namespace PersonDirectory.Repository
{
    internal sealed class RelatedPersonRepository : RepositoryBase<RelatedPersons>, IRelatedPersonRepository
    {
        private readonly DbSet<RelatedPersons> _relatedPersonsDbSet;

        internal RelatedPersonRepository(IPersonDirectoryDbContext context) : base(context)
        {
            _relatedPersonsDbSet = _context.Set<RelatedPersons>();
        }

        public async Task<IEnumerable<RelatedPersons>> GetAllPersonRelatedPersons(int personId) =>
            await _relatedPersonsDbSet.FromSqlRaw("EXEC GetAllPersonRelatedPersons @PersonId", new SqlParameter("@PersonId", personId)).ToListAsync();
    }
}
