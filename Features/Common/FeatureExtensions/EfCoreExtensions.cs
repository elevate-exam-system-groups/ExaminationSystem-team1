using System.Linq.Expressions;

namespace ExaminationSystem.Features.Common.FeatureExtensions
{
    public static class EfCoreExtensions
    {
        public static async Task<Dictionary<TKey, int>> CountByAsync<T, TKey>(
            this IQueryable<T> query,
            Expression<Func<T, TKey>> keySelector,
            CancellationToken ct = default) where TKey : notnull
        {
            return await query
                .GroupBy(keySelector)
                .Select(g => new
                {
                    g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(k => k.Key, v => v.Count, ct);
        }
        public static IQueryable<Quiz> Published(this IQueryable<Quiz> query)
            => query.Where(q => q.Status == QuizStatus.Published && !q.isDeleted);

        public static IQueryable<Diploma> Published(this IQueryable<Diploma> query)
            => query.Where(d => d.Status == DiplomaStatus.Published && !d.isDeleted);

        public static IQueryable<QuizAttempt> Completed(this IQueryable<QuizAttempt> query)
           => query.Where(a => a.Status != QuizAttemptStatus.InProgress
                            && !a.isDeleted);

    }
}
