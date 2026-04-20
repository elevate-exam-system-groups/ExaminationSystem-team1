using System.Linq.Expressions;
using ExaminationSystem.Domain.Models;

namespace ExaminationSystem.Domain.Abstractions
{
    public interface IGeneralRepository<T> where T : BaseModel
    {
        public IQueryable<T> GetAll();
        public IQueryable<T> GetById(Guid id);
        public IQueryable<T> Get(Expression<Func<T, bool>> expression);
        public IQueryable<T> GetByIdWithTracking(Guid id);

        public void Add(T entity);
        public Guid AddAndReturnId(T entity);
        public void Update(T entity);
        public void UpdateInclude(T entity, params string[] include);
        public void SoftDelete(T entity);
        public bool SoftDeleteById(Guid id);
        public void AddRange(IEnumerable<T> entities);

        public Task SaveChangesAsync();
    }
}
