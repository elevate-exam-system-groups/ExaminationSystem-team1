
using ExaminationSystem.Infrastructure.Data;

namespace ExaminationSystem.Infrastructure.Implementations
{
    public class GeneralRepository<TEntity, TKey> : IGeneralRepository<TEntity, TKey> where TEntity : BaseModel<TKey>
    {
        private readonly Context _dbContext;

        public GeneralRepository(Context dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
