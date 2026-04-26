using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs;
using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetAttemptsOverTime;
using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetAvgScoreByDiploma;
using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetPassRateByQuizQuery;
using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetTopFailedQuestions;
namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Orchestrator
{
    public class GetAnalyticsOrchestratorHandler 
        : IRequestHandler<GetAnalyticsOrchestrator, RequestResult<AnalyticsResponseDto>>
    {
        
        private readonly IMediator _mediator;
        public GetAnalyticsOrchestratorHandler(IMediator mediator)
            => _mediator = mediator;

        public async Task<RequestResult<AnalyticsResponseDto>> Handle(
            GetAnalyticsOrchestrator request, CancellationToken ct)
        {
            // 1️⃣ Pass Rate by Quiz
            var passRateResult = await _mediator.Send(
                new GetPassRateByQuizQuery(request.From, request.To), ct);

            if (!passRateResult.IsSuccess)
            {
                return RequestResult<AnalyticsResponseDto>.Failure(
                    passRateResult.Message ?? "Failed to load pass rate data",
                    passRateResult.requestErrorCode ?? RequestErrorCode.InternalServerError);
            }

            // 2️⃣ Avg Score by Diploma
            var avgScoreResult = await _mediator.Send(
                new GetAvgScoreByDiplomaQuery(request.From, request.To), ct);

            if (!avgScoreResult.IsSuccess)
            {
                return RequestResult<AnalyticsResponseDto>.Failure(
                    avgScoreResult.Message ?? "Failed to load avg score data",
                    avgScoreResult.requestErrorCode ?? RequestErrorCode.InternalServerError);
            }

            // 3️⃣ Attempts Over Time
            var attemptsResult = await _mediator.Send(
                new GetAttemptsOverTimeQuery(request.From, request.To), ct);

            if (!attemptsResult.IsSuccess)
            {
                return RequestResult<AnalyticsResponseDto>.Failure(
                    attemptsResult.Message ?? "Failed to load attempts over time data",
                    attemptsResult.requestErrorCode ?? RequestErrorCode.InternalServerError);
            }

            // 4️⃣ Top Failed Questions
            var failedQuestionsResult = await _mediator.Send(
                new GetTopFailedQuestionsQuery(request.From, request.To), ct);

            if (!failedQuestionsResult.IsSuccess)
            {
                return RequestResult<AnalyticsResponseDto>.Failure(
                    failedQuestionsResult.Message ?? "Failed to load failed questions data",
                    failedQuestionsResult.requestErrorCode ?? RequestErrorCode.InternalServerError);
            }

            // 5️⃣ Merge All Parts into One DTO
            var response = new AnalyticsResponseDto(
                PassRateByQuiz: passRateResult.Data!.PassRateByQuiz,
                AvgScoreByDiploma: avgScoreResult.Data!.AvgScoreByDiploma,
                AttemptsOverTime: attemptsResult.Data!.AttemptsOverTime,
                TopFailedQuestions: failedQuestionsResult.Data!.TopFailedQuestions
            );

            return RequestResult<AnalyticsResponseDto>.Success(response);
        }
    }
}