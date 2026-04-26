using System.Linq.Expressions;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>>? OrderBy { get; }
        bool IsDescending { get; }
        //int? Take { get; }
        //int? Skip { get; }

    }
}
