using ExaminationSystem.Domain.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExaminationSystem.Domain.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _dbContext;
        private IDbContextTransaction? _transaction;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(Context dbContext)
         => _dbContext = dbContext;


        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            return await _dbContext.Database.BeginTransactionAsync(ct);
        }

        #region Transaction Management

        //public async Task BeginTransactionAsync()
        //{
        //    _transaction = await _dbContext.Database.BeginTransactionAsync();
        //}

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        #endregion

        public IGeneralRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseModel
        {
            //Get the type of the entity
            var entityType = typeof(TEntity);

            //Check if the repository for this entity type already exists in dictionary
            if (_repositories.TryGetValue(entityType, out var repository))
                return (IGeneralRepository<TEntity>)repository;
            //If not, create a new repository instance
            var newRepo = new GeneralRepository<TEntity>(_dbContext);
            _repositories[entityType] = newRepo;
            return newRepo;
        }

        public Task<int> SaveChangesAsync()
            => _dbContext.SaveChangesAsync();
    }
}
