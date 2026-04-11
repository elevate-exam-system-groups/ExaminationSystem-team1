namespace ExaminationSystem.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IGeneralRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseModel;
    }
}
