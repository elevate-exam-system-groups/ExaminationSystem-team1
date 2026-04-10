
using ExaminationSystem.Infrastructure.Data;

namespace ExaminationSystem.Infrastructure.Implementations
{
    public class GeneralRepository<TEntity> : IGeneralRepository<TEntity> where TEntity : BaseModel
    {
        private readonly Context _dbContext;

        public GeneralRepository(Context dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
