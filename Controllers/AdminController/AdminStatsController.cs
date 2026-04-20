using ExaminationSystem.Controllers.QuestionController;
using ExaminationSystem.Features.Admin.DTOs;
using ExaminationSystem.Features.Admin;
using Microsoft.AspNetCore.Authorization;

namespace ExaminationSystem.Controllers.AdminController
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminStatsController : BaseApiController
    {
      
        private readonly IMediator _mediator;
        public AdminStatsController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("stats")]
        public async Task<ActionResult<ResponseViewModel<AdminStatsResponse>>> GetStats()
        {
            var result = await _mediator.Send(new GetAdminStatsQuery());
            return HandleResult(result);
        }

    }
}
