using ExaminationSystem.Controllers.AdminDashboard.Mapping;
using ExaminationSystem.Controllers.AdminDashboard.ViewModels;
using ExaminationSystem.Features.AdminDashboard.Orchestrator;
using Microsoft.AspNetCore.Authorization;

namespace ExaminationSystem.Controllers.AdminDashboard
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

            if (!result.IsSuccess)
                return HandleResult(RequestResult<AdminStatsVm>
                    .Failure(result.Message, result.requestErrorCode));

            return HandleResult(RequestResult<AdminStatsVm>
                .Success(AdminDashboardMapper.FromDto(result.Data!), result.Message));
        }
    }
}
