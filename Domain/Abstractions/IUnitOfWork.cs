using ExaminationSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExaminationSystem.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IGeneralRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseModel;
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);

    }
}

