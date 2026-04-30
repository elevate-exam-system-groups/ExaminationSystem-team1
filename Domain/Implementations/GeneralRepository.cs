using ExaminationSystem.Domain.Data;
using System.Linq.Expressions;

namespace ExaminationSystem.Domain.Implementations
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : BaseModel
    {

        Context _context;
        DbSet<T> _dbSet;
        public GeneralRepository(Context context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }


        #region ReadRegion
        public IQueryable<T> GetAll()
        {
            return _dbSet.Where(x => !x.IsDeleted);
        }

        public IQueryable<T> GetById(Guid id)
        {
            return _dbSet.Where(x => x.Id == id && !x.IsDeleted);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            return GetAll().Where(expression);
        }
        public IQueryable<T> GetByIdWithTracking(Guid id)
        {
            var trackedEntity = _dbSet.Where(x => x.Id == id && !x.IsDeleted)
                .AsTracking();

            return trackedEntity;
        }

        #endregion

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            entity.CreatedAt = DateTime.UtcNow;
        }

        public Guid AddAndReturnId(T entity)
        {
            _dbSet.Add(entity);
            entity.CreatedAt = DateTime.UtcNow;
            return entity.Id;
        }

        #region UpdateRegion

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            entity.UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateInclude(T entity, params string[] include)
        {
            if (!_dbSet.Any(e => e.Id == entity.Id && !e.IsDeleted))
            {
                return;
            }

            //1-Check if the entity is already being tracked by the context
            var local = _dbSet.Local.FirstOrDefault(entry => entry.Id == entity.Id);

            EntityEntry entityEntry;


            if (local is null)
            {
                // 2- If the entity is not being tracked, attach it to the context
                entityEntry = _context.Entry(entity); //start tracking the entity

            }
            else
            {
                // 3- If the entity is already being tracked, use the existing tracked entity
                entityEntry = _context.ChangeTracker.Entries<T>()
                                                    .FirstOrDefault(e => e.Entity.Id == entity.Id);
            }

            // 4- Mark only the specified properties as modified
            foreach (var prop in entityEntry.Properties)
            {
                if (include.Contains(prop.Metadata.Name))
                {
                    // Set the current value of the property to the value from the provided entity
                    prop.CurrentValue = entity.GetType()
                                              .GetProperty(prop.Metadata.Name)
                                              .GetValue(entity);
                    prop.IsModified = true;

                }

            }
        }

        #endregion

        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
            UpdateInclude(entity, nameof(entity.IsDeleted));
            entity.DeletedAt = DateTime.UtcNow;
        }

        public bool SoftDeleteById(Guid id)
        {
            var entity = GetById(id);
            if (entity is null)
                return false;
            SoftDelete(entity.FirstOrDefault());
            return true;
        }

        public void AddRange(IEnumerable<T> entities)
         => _dbSet.AddRange(entities);



        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();

        }
    }
}
