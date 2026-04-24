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
        public async Task<ResponseViewModel<AdminStatsVm>> GetStats()
        {
            var result = await _mediator.Send(new GetAdminStatsOrchestrator());

            if (!result.IsSuccess)
                return ResponseViewModel<AdminStatsVm>.Failure(
                    result.Message ?? "Failed to retrieve stats",
                    ResponseVmErrorCode.InternalServerError);

            var data = result.Data;

            return ResponseViewModel<AdminStatsVm>.Success(
                AdminDashboardMapper.FromDto(data));
        }
    }
}
