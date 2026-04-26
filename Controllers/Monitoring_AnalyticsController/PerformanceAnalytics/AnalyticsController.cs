using ExaminationSystem.Controllers.Monitoring_AnalyticsController.PerformanceAnalytics.Mapping;
using ExaminationSystem.Controllers.Monitoring_AnalyticsController.PerformanceAnalytics.ViewModels;
using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Orchestrator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Controllers.Monitoring_AnalyticsController.PerformanceAnalytics
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AnalyticsController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;

        public AnalyticsController(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        [HttpGet("analytics")]
        public async Task<ActionResult<ResponseViewModel<AnalyticsResponseVm>>> GetAnalytics(
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null)
        {
            var cacheKey = $"analytics_{from?.ToString("yyyyMMdd") ?? "all"}_{to?.ToString("yyyyMMdd") ?? "all"}";

            if (_cache.TryGetValue(cacheKey, out AnalyticsResponseVm? cached))
                return Ok(ResponseViewModel<AnalyticsResponseVm>.Success(cached!));

            var query = new GetAnalyticsOrchestrator(From: from, To: to);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return HandleResult(RequestResult<AnalyticsResponseVm>.Failure(
                    result.Message!, result.requestErrorCode ?? RequestErrorCode.InternalServerError));
            }

            var vm = result.Data!.ToViewModel();
            _cache.Set(cacheKey, vm, TimeSpan.FromMinutes(10));

            return HandleResult(RequestResult<AnalyticsResponseVm>.Success(vm));
        }
    }
}