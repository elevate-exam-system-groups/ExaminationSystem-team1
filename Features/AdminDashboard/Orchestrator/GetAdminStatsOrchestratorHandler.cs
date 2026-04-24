using ExaminationSystem.Features.Admin.DTOs;
using ExaminationSystem.Features.Admin.Queries.GetActiveUsersToday;
using ExaminationSystem.Features.Admin.Queries.GetAttemptsAvgPassRate;
using ExaminationSystem.Features.Admin.Queries.GetTotalQuizzes;
using ExaminationSystem.Features.Admin.Queries.GetTotalUsers;
using ExaminationSystem.Features.Common.Request;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Features.Admin.Orchestrator
{
    public class GetAdminStatsOrchestratorHandler
        : IRequestHandler<GetAdminStatsOrchestrator, RequestResult<AdminStatsResponseDto>>
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "admin_dashboard_stats";

        public GetAdminStatsOrchestratorHandler(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<RequestResult<AdminStatsResponseDto>> Handle
            (GetAdminStatsOrchestrator request, CancellationToken ct)
        {

            if (_cache.TryGetValue(CacheKey, out AdminStatsResponseDto? cached))
                return RequestResult<AdminStatsResponseDto>.Success(cached!);

            var usersResult = await _mediator.Send(new GetTotalUsersQuery(), ct);
            var activeResult = await _mediator.Send(new GetActiveUsersTodayQuery(), ct);
            var quizzesResult = await _mediator.Send(new GetTotalQuizzesQuery(), ct);
            var attemptsResult = await _mediator.Send(new GetAttemptsAvgPassRateQuery(), ct);

            if (!usersResult.IsSuccess || !activeResult.IsSuccess ||
                !quizzesResult.IsSuccess || !attemptsResult.IsSuccess)
                return RequestResult<AdminStatsResponseDto>.Failure
                    ("Failed to retrieve stats", RequestErrorCode.InternalServerError);

            var response = new AdminStatsResponseDto(
                usersResult.Data.TotalUsers,
                activeResult.Data.ActiveUsersToday,
                quizzesResult.Data.TotalQuizzes,
                attemptsResult.Data.TotalAttempts,
                attemptsResult.Data.AvgPassRate
            );

            _cache.Set(CacheKey, response, TimeSpan.FromMinutes(5));
            return RequestResult<AdminStatsResponseDto>.Success(response);
        }
    }
}
