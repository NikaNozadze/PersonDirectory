using PersonDirectory.Interfaces.DbContext;
using PersonDirectory.Interfaces.Repositories;

namespace PersonDirectory.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IPersonDirectoryDbContext _context;
        private readonly Lazy<IPersonRepository> _personRepository;
        private readonly Lazy<IPhoneNumberRepository> _phoneNumberRepository;
        private readonly Lazy<IRelatedPersonRepository> _relatedPersonRepository;
        private readonly Lazy<ICityRepository> _cityRepository;
        private readonly Lazy<ISearchRepository> _searchRepository;

        public UnitOfWork(IPersonDirectoryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _personRepository = new Lazy<IPersonRepository>(() => new PersonRepository(context));
            _phoneNumberRepository = new Lazy<IPhoneNumberRepository>(() => new PhoneNumberRepository(context));
            _relatedPersonRepository = new Lazy<IRelatedPersonRepository>(() => new RelatedPersonRepository(context));
            _cityRepository = new Lazy<ICityRepository>(() => new CityRepository(context));
            _searchRepository = new Lazy<ISearchRepository>(() => new SearchRepository(context));
        }

        public IPersonRepository PersonRepository => _personRepository.Value;

        public IPhoneNumberRepository PhoneNumberRepository => _phoneNumberRepository.Value;

        public IRelatedPersonRepository RelatedPersonRepository => _relatedPersonRepository.Value;

        public ICityRepository CityRepository => _cityRepository.Value;

        public ISearchRepository SearchRepository => _searchRepository.Value;

        public int SaveChanges() => _context.SaveChanges();

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void BeginTransaction()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                throw new InvalidOperationException("A Transaction is already in progress.");
            }

            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            try
            {
                _context.Database.CurrentTransaction?.Commit();
            }
            catch
            {
                _context.Database.CurrentTransaction?.Rollback();
                throw;
            }
            finally
            {
                _context.Database.CurrentTransaction?.Dispose();
            }
        }

        public void RollBack()
        {
            try
            {
                _context.Database.CurrentTransaction?.Rollback();
            }
            finally
            {
                _context.Database.CurrentTransaction?.Dispose();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
