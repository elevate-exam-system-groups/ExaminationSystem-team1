using ExaminationSystem.Domain.Data;

namespace ExaminationSystem.Domain.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _dbContext;

        public UnitOfWork(Context dbContext)
         => _dbContext = dbContext;

        public Task<int> SaveChangesAsync()
            => _dbContext.SaveChangesAsync();
    }
}
