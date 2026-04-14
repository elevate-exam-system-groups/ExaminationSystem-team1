using System.Linq.Expressions;

namespace ExaminationSystem.Domain.Abstractions
{
    public interface IGeneralRepository<T> where T : BaseModel
    {
        // Read
        public IQueryable<T> GetAll();
        public IQueryable<T> GetById(Guid id);
        public IQueryable<T> Get(Expression<Func<T, bool>> expression);
        public IQueryable<T> GetByIdWithTracking(Guid id);

        // Write
        public void Add(T entity);
        public void Update(T entity);
        public void UpdateInclude(T entity, params string[] include);
        public void SoftDelete(T entity);
        public void AddRange(IEnumerable<T> entities);
    }
}
