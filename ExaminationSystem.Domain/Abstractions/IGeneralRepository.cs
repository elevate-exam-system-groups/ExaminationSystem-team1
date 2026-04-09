
namespace ExaminationSystem.Domain.Abstractions
{
    public interface IGeneralRepository<TEntity, TKey> where TEntity : BaseModel<TKey>
    {
    }
}
