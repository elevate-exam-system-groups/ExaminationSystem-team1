namespace ExaminationSystem.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task ExecuteAsync(Func<Task> action, CancellationToken cs);
        Task CreateSavePoint(string savePointName, CancellationToken cs);

    }
}
