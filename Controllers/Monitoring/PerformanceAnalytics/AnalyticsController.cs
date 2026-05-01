using ExaminationSystem.Controllers.AdminManagementControllers.PerformanceAnalytics.Filter;
using ExaminationSystem.Controllers.AdminManagementControllers.PerformanceAnalytics.Mapping;
using ExaminationSystem.Controllers.AdminManagementControllers.PerformanceAnalytics.ViewModels;
using ExaminationSystem.Features.Monitoring.PerformanceAnalytics.Orchestrator.GetAnalytics;
using Microsoft.AspNetCore.Authorization;

namespace ExaminationSystem.Controllers.AdminManagementControllers.PerformanceAnalytics
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AnalyticsController : BaseApiController
    {

        private readonly IMediator _mediator;
        public AnalyticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("analytics")]
        public async Task<ActionResult<ResponseViewModel<AnalyticsResponseVm>>> GetAnalytics(
            [FromQuery] AnalyticsFilterRequest filter)
        {

            if (filter.From.HasValue && filter.To.HasValue && filter.From > filter.To)
                return BadRequest(ResponseViewModel<AnalyticsResponseVm>
                    .Failure("From date must be before To date"));

            var result = await _mediator.Send(new GetAnalyticsOrchestrator(filter.From, filter.To));

            if (!result.IsSuccess)
            {
                return HandleResult(RequestResult<AnalyticsResponseVm>.Failure(
                    result.Message!,
                    result.requestErrorCode ?? RequestErrorCode.InternalServerError));
            }

            var vm = result.Data!.ToViewModel();
            return HandleResult(RequestResult<AnalyticsResponseVm>.Success(vm));
        }
    }
}