using ExaminationSystem.Controllers.AdminController.Mapping;
using ExaminationSystem.Controllers.AdminController.ViewModels;
using ExaminationSystem.Features.Admin.Orchestrator;
using Microsoft.AspNetCore.Authorization;

namespace ExaminationSystem.Controllers.AdminController
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : BaseApiController
    {

        private readonly IMediator _mediator;
        public AdminDashboardController(IMediator mediator)
            => _mediator = mediator;


        [HttpGet]
        public async Task<ActionResult<ResponseViewModel<AdminStatsVm>>> GetStats()
        {
            var result = await _mediator.Send(new GetAdminStatsOrchestrator());

            var mappedResult = result.IsSuccess ?
                RequestResult<AdminStatsVm>.Success(
                    AdminDashboardMapper.FromDto(result.Data), result.Message)
                : RequestResult<AdminStatsVm>.Failure(
                    result.Message, result.requestErrorCode);

            return HandleResult(mappedResult);
        }
    }
}
