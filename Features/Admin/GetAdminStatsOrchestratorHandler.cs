using ExaminationSystem.Features.Admin.DTOs;
using ExaminationSystem.Features.Admin.Queries;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Features.Admin
{
    public class GetAdminStatsQueryHandler
       : IRequestHandler<GetAdminStatsQuery, RequestResult<AdminStatsResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "Admin_Dashboard_Stats";

        public GetAdminStatsQueryHandler(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<RequestResult<AdminStatsResponse>> Handle(
            GetAdminStatsQuery request, CancellationToken ct)
        {
            // 1️⃣ Try Cache
            if (_cache.TryGetValue(CacheKey, out AdminStatsResponse? cached))
                return RequestResult<AdminStatsResponse>.Success(cached!);

            // 2️⃣ Execute Parallel Tasks
            var usersTask = _mediator.Send(new GetTotalUsersQuery(), ct);
            var activeTask = _mediator.Send(new GetActiveUsersTodayQuery(), ct);
            var quizStatsTask = _mediator.Send(new GetGlobalQuizStatsQuery(), ct);

            await Task.WhenAll(usersTask, activeTask, quizStatsTask);

            // 3️⃣ Await results properly
            var totalUsers = await usersTask;
            var activeUsers = await activeTask;
            var quizStats = await quizStatsTask;

            // 4️⃣ Build response
            var response = new AdminStatsResponse(
                totalUsers,
                activeUsers,
                quizStats.TotalQuizzes,
                quizStats.TotalAttempts,
                quizStats.AvgPassRate
            );

            // 5️⃣ Cache for 5 minutes
            _cache.Set(CacheKey, response, TimeSpan.FromMinutes(5));

            return RequestResult<AdminStatsResponse>.Success(response);
        }
    }
}
