using ExaminationSystem.Controllers.QuestionController;
using ExaminationSystem.Features.StudentDashboard;
using ExaminationSystem.Features.StudentDashboard.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ExaminationSystem.Controllers.StudentController
{
    [Route("api/student")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentDashboardController : BaseApiController
    {

        private readonly IMediator _mediator;
        public StudentDashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<ResponseViewModel<StudentDashboardResponse>>> GetDashboard()
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(studentId))
                return Unauthorized(ResponseViewModel<StudentDashboardResponse>.Failure(
                    "Invalid token", ResponseVmErrorCode.Unauthorized));

            var result = await _mediator.Send(new GetStudentDashboardOrchestrator(studentId));

            return HandleResult(result);
        }
    }
}