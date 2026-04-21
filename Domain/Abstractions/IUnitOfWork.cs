using ExaminationSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExaminationSystem.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}

