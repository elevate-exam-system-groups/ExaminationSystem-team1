using ExaminationSystem.Controllers.StudentController.Mapping;
using ExaminationSystem.Controllers.StudentController.ViewModels;
using ExaminationSystem.Features.StudentDashboard.Orchestrator.StudentDashboard;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ExaminationSystem.Controllers.StudentController
{
    namespace ExaminationSystem.Controllers.StudentController
    {

        [Route("api/student")]
        [ApiController]
        [Authorize(Roles = "Student")]
        public class StudentDashboardController : BaseApiController
        {

            private readonly IMediator _mediator;
            public StudentDashboardController(IMediator mediator)
                => _mediator = mediator;


            public async Task<ActionResult<ResponseViewModel<StudentDashboardResponseVm>>> GetDashboard()
            {
                var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _mediator.Send(
                    new GetStudentDashboardOrchestrator(studentId));

                var mappedResult = result.IsSuccess
                    ? RequestResult<StudentDashboardResponseVm>.Success(
                        result.Data.ToViewModel(), result.Message)
                    : RequestResult<StudentDashboardResponseVm>.Failure(
                        result.Message, result.requestErrorCode);

                return HandleResult(mappedResult);
            }
        }
    }
}
