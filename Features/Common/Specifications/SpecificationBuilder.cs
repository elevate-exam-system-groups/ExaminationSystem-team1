using System.Linq.Expressions;

namespace ExaminationSystem.Features.Common.Specifications
{
    public class SpecificationBuilder<T> where T : BaseModel
    {
        private Expression<Func<T, bool>>? _criteria;

        public SpecificationBuilder<T> Add(Expression<Func<T, bool>> criteria)
        {
            _criteria = _criteria == null ? criteria : _criteria.And(criteria);
            return this;
        }

        public Expression<Func<T, bool>>? Build()
        {
            return _criteria;
        }
    }

    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(T));

            var body = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
