using System.Linq.Expressions;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; protected set; } = null!;
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        public Expression<Func<T, object>>? OrderBy { get; protected set; }
        public bool IsDescending { get; protected set; }
        //public int? Take { get; private set; }
        //public int? Skip { get; private set; }

        public BaseSpecification() { }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected void AddInclude(Expression<Func<T, object>> include)
            => Includes.Add(include);

        protected void ApplyOrderBy(Expression<Func<T, object>> orderBy, bool isDescending = false)
        {
            OrderBy = orderBy;
            IsDescending = isDescending;
        }

        //protected void ApplyPaging(int skip, int take)
        //{
        //    Skip = skip;
        //    Take = take;
        //}
    }
}
