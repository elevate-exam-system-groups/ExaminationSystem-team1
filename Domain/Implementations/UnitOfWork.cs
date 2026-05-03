using Microsoft.EntityFrameworkCore.Storage;

namespace ExaminationSystem.Domain.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Data.IUnitOfWork _context;
        IDbContextTransaction _transaction;
        short _depth = 0;
        string _savePointName = string.Empty;

        public UnitOfWork(Data.IUnitOfWork context, IDbContextTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public async Task CreateSavePoint(string SavePointName, CancellationToken cs)
        {
            _savePointName = SavePointName;
            if (_transaction is null)
            {
                _transaction = await _context.Database.BeginTransactionAsync(cs);
            }
            await _transaction.CreateSavepointAsync(SavePointName, cs);
        }

        public async Task ExecuteAsync(Func<Task> action, CancellationToken cs)
        {
            if (_transaction is null)
            {
                _transaction = await _context.Database.BeginTransactionAsync(cs);
            }
            try
            {
                _depth++;

                await action();

                if (_depth == 0)
                    await _transaction.CommitAsync(cs);
            }
            catch
            {
                //await _transaction.RollbackAsync(cs);
                await _transaction.RollbackToSavepointAsync(_savePointName, cs);
                throw;
            }
            finally
            {
                if (_depth == 0 && _transaction is not null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }

                _depth--;

            }

        }

    }
}
