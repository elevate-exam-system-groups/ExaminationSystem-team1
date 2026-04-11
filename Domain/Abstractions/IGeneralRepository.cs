

using System.Linq.Expressions;

namespace ExaminationSystem.Domain.Abstractions
{
    public interface IGeneralRepository<T> where T : BaseModel
    {
        public IQueryable<T> GetAll();

        public IQueryable<T> GetById(int id);

        public IQueryable<T> Get(Expression<Func<T, bool>> expression);
        public IQueryable<T> GetByIdWithTracking(int id);
        public void Add(T entity);
        public void Update(T entity);
        public void UpdateInclude(T entity, params string[] include);
        public void SoftDelete(T entity);
    }
}
