namespace PersonDirectory.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPersonRepository PersonRepository { get; }
        IPhoneNumberRepository PhoneNumberRepository { get; }
        IRelatedPersonRepository RelatedPersonRepository { get; }
        ICityRepository CityRepository { get; }
        ISearchRepository SearchRepository { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
