using ExaminationSystem.Features.Admin.DTOs;
using ExaminationSystem.Features.Admin.Queries;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Features.Admin
{
    public class GetAdminStatsOrchestratorHandler : IRequestHandler<GetAdminStatsOrchestrator, RequestResult<AdminStatsResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "admin_dashboard_stats";

        public GetAdminStatsOrchestratorHandler(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<RequestResult<AdminStatsResponse>> Handle
            (GetAdminStatsOrchestrator request, CancellationToken ct)
        {

            if (_cache.TryGetValue(CacheKey, out AdminStatsResponse? cached))
                return RequestResult<AdminStatsResponse>.Success(cached!);

            var usersResult = await _mediator.Send(new GetTotalUsersQuery(), ct);
            var activeResult = await _mediator.Send(new GetActiveUsersTodayQuery(), ct);
            var quizzesResult = await _mediator.Send(new GetTotalQuizzesQuery(), ct);
            var attemptsResult = await _mediator.Send(new GetGlobalAttemptStatsQuery(), ct);

            if (!usersResult.IsSuccess || !activeResult.IsSuccess ||
                !quizzesResult.IsSuccess || !attemptsResult.IsSuccess)
                return RequestResult<AdminStatsResponse>.Failure
                    ("Failed to retrieve stats", RequestErrorCode.InternalServerError);

            var response = new AdminStatsResponse(
                usersResult.Data,
                activeResult.Data,
                quizzesResult.Data,
                attemptsResult.Data.TotalAttempts,
                attemptsResult.Data.AvgPassRate
            );

            _cache.Set(CacheKey, response, TimeSpan.FromMinutes(5));
            return RequestResult<AdminStatsResponse>.Success(response);
        }
    }
}
