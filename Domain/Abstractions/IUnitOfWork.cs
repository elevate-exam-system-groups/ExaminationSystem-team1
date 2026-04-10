namespace ExaminationSystem.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IGeneralRepository<T> GetRepository<T>() where T : BaseModel;
    }
}
