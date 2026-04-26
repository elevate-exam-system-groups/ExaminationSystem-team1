namespace ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> ApplySpecification<T>(
            this IQueryable<T> query, ISpecification<T> spec) where T : BaseModel
        {

            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            query = spec.Includes.Aggregate(query, (current, include)
                => current.Include(include));

            if (spec.OrderBy != null)
            {
                query = spec.IsDescending ?
                    query.OrderByDescending(spec.OrderBy)
                    : query.OrderBy(spec.OrderBy);
            }

            return query;
        }

    }
}
