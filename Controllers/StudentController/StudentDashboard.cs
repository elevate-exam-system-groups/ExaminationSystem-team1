using ExaminationSystem.Controllers.QuestionController;
using ExaminationSystem.Controllers.StudentController.ViewModels;
using ExaminationSystem.Features.StudentDashboard;
using ExaminationSystem.Features.StudentDashboard.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ExaminationSystem.Controllers.StudentController
{
    [Route("api/student")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentDashboardController : ControllerBase
    {

        private readonly IMediator _mediator;
        public StudentDashboardController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("dashboard")]
        public async Task<ActionResult<ResponseViewModel<StudentDashboardResponseVm>>> GetDashboard()
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(studentId))
                return Unauthorized(ResponseViewModel<StudentDashboardResponseVm>.Failure(
                    "Invalid token", ResponseVmErrorCode.Unauthorized));

            var result = await _mediator.Send(new GetStudentDashboardOrchestrator(studentId));

            if (!result.IsSuccess)
                return BadRequest(ResponseViewModel<StudentDashboardResponseVm>.Failure(
                    result.Message ?? "Failed to load dashboard"));

            var vm = new StudentDashboardResponseVm(
                result.Data.EnrolledDiplomas.Select(d => new EnrolledDiplomaVm(
                    d.DiplomaId, d.Title, d.Description,
                    d.TotalQuizzes, d.CompletedQuizzes, d.ProgressPercentage
                )).ToList(),
                result.Data.RecentQuizAttempts.Select(a => new RecentAttemptVm(
                    a.AttemptId, a.QuizId, a.QuizTitle, a.DiplomaTitle,
                    a.Status, a.Score, a.IsPassed, a.SubmittedAt
                )).ToList(),
                new OverallStatsVm(
                    result.Data.OverallStats.TotalQuizzesTaken,
                    result.Data.OverallStats.AvgScore,
                    result.Data.OverallStats.PassRate
                )
            );

            return Ok(ResponseViewModel<StudentDashboardResponseVm>.Success(vm));
        }
    }
}