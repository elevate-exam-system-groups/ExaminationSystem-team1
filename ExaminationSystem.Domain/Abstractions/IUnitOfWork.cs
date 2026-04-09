namespace ExaminationSystem.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IGeneralRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseModel<TKey>;
    }
}
