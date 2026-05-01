using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;
using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetAttemptsOverTime;
using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetAvgScoreByDiploma;
using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetPassRateByQuizQuery;
using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetTopFailedQuestionsQuery;
using Microsoft.Extensions.Caching.Memory;
namespace ExaminationSystem.Features.Monitoring.PerformanceAnalytics.Orchestrator.GetAnalytics
{
    public class GetAnalyticsOrchestratorHandler
          : IRequestHandler<GetAnalyticsOrchestrator, RequestResult<AnalyticsResponseDto>>
    {

        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;

        public GetAnalyticsOrchestratorHandler(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<RequestResult<AnalyticsResponseDto>> Handle(
            GetAnalyticsOrchestrator request, CancellationToken ct)
        {

            var cacheKey = $"analytics_{request.From?.ToString("yyyyMMdd") ?? "all"}_{request.To?.ToString("yyyyMMdd") ?? "all"}";

            if (_cache.TryGetValue(cacheKey, out AnalyticsResponseDto? cachedData))
            {
                return RequestResult<AnalyticsResponseDto>.Success(cachedData!);
            }


            var passRateResult = await _mediator.Send(
                new GetPassRateByQuizQuery(request.From, request.To), ct);

            if (!passRateResult.IsSuccess)
            {
                return RequestResult<AnalyticsResponseDto>.Failure(
                    passRateResult.Message ?? "Failed to load pass rate data",
                    passRateResult.requestErrorCode ?? RequestErrorCode.InternalServerError);
            }

            var avgScoreResult = await _mediator.Send(
                new GetAvgScoreByDiplomaQuery(request.From, request.To), ct);

            if (!avgScoreResult.IsSuccess)
            {
                return RequestResult<AnalyticsResponseDto>.Failure(
                    avgScoreResult.Message ?? "Failed to load avg score data",
                    avgScoreResult.requestErrorCode ?? RequestErrorCode.InternalServerError);
            }

            var attemptsResult = await _mediator.Send(
                new GetAttemptsOverTimeQuery(request.From, request.To), ct);

            if (!attemptsResult.IsSuccess)
            {
                return RequestResult<AnalyticsResponseDto>.Failure(
                    attemptsResult.Message ?? "Failed to load attempts over time data",
                    attemptsResult.requestErrorCode ?? RequestErrorCode.InternalServerError);
            }

            var failedQuestionsResult = await _mediator.Send(
                new GetTopFailedQuestionsQuery(request.From, request.To), ct);

            if (!failedQuestionsResult.IsSuccess)
            {
                return RequestResult<AnalyticsResponseDto>.Failure(
                    failedQuestionsResult.Message ?? "Failed to load failed questions data",
                    failedQuestionsResult.requestErrorCode ?? RequestErrorCode.InternalServerError);
            }

            var response = new AnalyticsResponseDto(
                passRateResult.Data,
                avgScoreResult.Data,
                attemptsResult.Data,
                failedQuestionsResult.Data
            );

            _cache.Set(cacheKey, response, TimeSpan.FromMinutes(10));

            return RequestResult<AnalyticsResponseDto>.Success(response);
        }
    }
}

