using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.DbContext;
using PersonDirectory.Interfaces.Repositories;
using PersonDirectory.Model;

namespace PersonDirectory.Repository
{
    internal sealed class SearchRepository : RepositoryBase<Person>, ISearchRepository
    {
        private readonly DbSet<Person> _personDbSet;
        private readonly DbSet<City> _cityDbSet;
        private readonly DbSet<PhoneNumber> _phoneNumberDbSet;

        internal SearchRepository(IPersonDirectoryDbContext context) : base(context)
        {
            _personDbSet = _context.Set<Person>();
            _cityDbSet = _context.Set<City>();
            _phoneNumberDbSet = _context.Set<PhoneNumber>();
        }

        public async Task<IEnumerable<Person>> SearchPersonsAsync(string searchTerm) =>
            await _personDbSet.FromSqlRaw("EXEC SearchPersons @SearchTerm", new SqlParameter("@SearchTerm", searchTerm)).ToListAsync();
        public async Task<IEnumerable<City>> SearchCitiesAsync(string searchTerm) =>
            await _cityDbSet.FromSqlRaw("EXEC SearchCities @SearchTerm", new SqlParameter("@SearchTerm", searchTerm)).ToListAsync();
        public async Task<IEnumerable<PhoneNumber>> SearchPhoneNumbersAsync(string searchTerm) =>
            await _phoneNumberDbSet.FromSqlRaw("EXEC SearchPhoneNumbers @SearchTerm", new SqlParameter("@SearchTerm", searchTerm)).ToListAsync();

        public async Task<IEnumerable<Person>> DetailedSearchPersonsAsync(SearchParametersModel parameters)
        {
            var firstNameParam = new SqlParameter("@FirstName", (object)parameters.FirstName ?? DBNull.Value);
            var lastNameParam = new SqlParameter("@LastName", (object)parameters.LastName ?? DBNull.Value);
            var genderParam = new SqlParameter("@Gender", (object)parameters.Gender ?? DBNull.Value);
            var personalNumberParam = new SqlParameter("@PersonalNumber", (object)parameters.PersonalNumber ?? DBNull.Value);
            var dateOfBirthParam = new SqlParameter("@DateOfBirth", (object)parameters.DateOfBirth ?? DBNull.Value);
            var cityIdParam = new SqlParameter("@CityId", (object)parameters.CityId ?? DBNull.Value);

            return await _dbSet.FromSqlRaw("EXEC DetailedSearchPersons @FirstName, @LastName, @Gender, @PersonalNumber, @DateOfBirth, @CityId",
                                firstNameParam, lastNameParam, genderParam, personalNumberParam, dateOfBirthParam, cityIdParam)
                               .ToListAsync();
        }
    }
}
