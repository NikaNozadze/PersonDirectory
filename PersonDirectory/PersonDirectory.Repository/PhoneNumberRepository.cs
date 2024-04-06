using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.DbContext;
using PersonDirectory.Interfaces.Repositories;

namespace PersonDirectory.Repository
{
    internal sealed class PhoneNumberRepository : RepositoryBase<PhoneNumber>, IPhoneNumberRepository
    {
        private readonly DbSet<PhoneNumber> _phoneNumberDbSet;

        internal PhoneNumberRepository(IPersonDirectoryDbContext context) : base(context)
        {
            _phoneNumberDbSet = _context.Set<PhoneNumber>();
        }

        public async Task<IEnumerable<PhoneNumber>> GetAllPersonPhoneNumbers(int personId) =>
            await _phoneNumberDbSet.FromSqlRaw("EXEC GetAllPersonPhoneNumbers @PersonId", new SqlParameter("@PersonId", personId)).ToListAsync();
    }
}
