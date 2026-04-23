using ExaminationSystem.Controllers.AttemptController.ViewModels;
using ExaminationSystem.Features.Attempts.SubmitQuizAttempt.Orchestrators;


namespace ExaminationSystem.Controllers.AttemptController
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AttemptController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttemptController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ResponseViewModel<SubmitAttemptResultVM>> SubmitQuizAttempt(Guid AttemptId, string StudentId, CancellationToken cancellationToken)
        {
            var result = await _mediator
                .Send(new SubmitQuizAttemptOrchestrator(StudentId, AttemptId), cancellationToken);

            if (!result.IsSuccess)
            {
                return ResponseViewModel<SubmitAttemptResultVM>
                    .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode);
            }

            return ResponseViewModel<SubmitAttemptResultVM>
                .Success(new SubmitAttemptResultVM
                (
                    result.Data.Score,
                    result.Data.IsPassed,
                    result.Data.Status
                ));
        }
    }
}
