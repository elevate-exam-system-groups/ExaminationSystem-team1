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
            => query.Where(q => q.Status == QuizStatus.Published && !q.IsDeleted);


    }
}
