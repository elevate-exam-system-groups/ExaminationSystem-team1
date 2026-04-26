using ExaminationSystem.Controllers.Monitoring_Analytics.ViewStudentAttempts.Mapping;
using ExaminationSystem.Controllers.Monitoring_Analytics.ViewStudentAttempts.ViewModels;
using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Queries.GetAttemptDetail;
using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Queries.GetAttempts;
using Microsoft.AspNetCore.Authorization;

namespace ExaminationSystem.Controllers.Monitoring_Analytics.ViewStudentAttempts
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AttemptsController : BaseApiController
    {

        private readonly IMediator _mediator;
        public AttemptsController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("attempts")]
        public async Task<ActionResult<ResponseViewModel<PaginatedResponseVm<AttemptVm>>>> GetAttempts(
            [FromQuery] Guid? quiz_id = null,
            [FromQuery] string? student_id = null,
            [FromQuery] string? status = null,
            [FromQuery] string? sort_by = "submitted_at",
            [FromQuery] string? order = "desc",
            [FromQuery] int page = 1,
            [FromQuery] int page_size = 20)
        {

            var query = new GetAttemptsQuery(
                QuizId: quiz_id,
                StudentId: student_id,
                Status: status,
                SortBy: sort_by,
                order: order,
                Page: page,
                PageSize: page_size
            );

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return HandleResult(RequestResult<PaginatedResponseVm<AttemptVm>>.Failure(
                    result.Message, result.requestErrorCode));
            }

            return HandleResult(RequestResult<PaginatedResponseVm<AttemptVm>>.Success(
                result.Data!.ToViewModel()));
        }

        [HttpGet("attempts/{attemptId:guid}")]
        public async Task<ActionResult<ResponseViewModel<AttemptDetailVm>>> GetAttemptDetail(
            Guid attemptId)
        {
            var result = await _mediator.Send(new GetAttemptDetailQuery(attemptId));

            if (!result.IsSuccess)
            {
                return HandleResult(RequestResult<AttemptDetailVm>.Failure(
                    result.Message, result.requestErrorCode));
            }

            return HandleResult(RequestResult<AttemptDetailVm>.Success(
                result.Data!.ToViewModel()));
        }
    }
}
