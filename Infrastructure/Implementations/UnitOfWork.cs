namespace ExaminationSystem.Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(Context dbContext)
        {
            _dbContext = dbContext;
        }
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
