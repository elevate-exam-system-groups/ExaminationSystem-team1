using ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.Filter;
using ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.Mapping;
using ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.Pagination;
using ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.ViewModels;
using ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Queries.GetAllAttempts;
using ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Queries.GetAttemptDetail;
using Microsoft.AspNetCore.Authorization;

namespace ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts
{

    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminAttemptsController : BaseApiController
    {

        private readonly IMediator _mediator;
        public AdminAttemptsController(IMediator mediator)
            => _mediator = mediator;


        [HttpGet("attempts")]
        public async Task<ActionResult<ResponseViewModel<PaginatedResponseVm<AttemptVm>>>> GetAttempts(
            [FromQuery] AttemptFilterRequest filter)
        {
            var query = new GetAllAttemptsQuery(
                filter.QuizId,
                filter.StudentId,
                filter.Status,
                filter.SortBy,
                filter.Order,
                filter.Page,
                filter.PageSize
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