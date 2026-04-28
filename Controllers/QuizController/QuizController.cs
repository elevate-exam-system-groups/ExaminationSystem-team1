
using ExaminationSystem.Controllers.QuizController.ViewModels;
using ExaminationSystem.Features.QuizFeatures.CreateQuiz.Commands;
using ExaminationSystem.Features.QuizFeatures.PublishQuiz.Orchestrators;
using ExaminationSystem.Features.QuizFeatures.UnpublishQuiz.Orchestrators;

namespace ExaminationSystem.Controllers.QuizController
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuizController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ResponseViewModel<Guid?>> CreateQuiz([FromBody] CreateQuizRequestVM NewQuizVM, CancellationToken ct)
        {
            var requestResult = await _mediator
                .Send(new CreateQuizCommand(
                NewQuizVM.Title,
                NewQuizVM.DiplomaId,
                NewQuizVM.DurationInMinutes,
                NewQuizVM.PassScore,
                NewQuizVM.MaxAttempts,
                NewQuizVM.Instructions),
                ct);

            if (!requestResult.IsSuccess)
            {
                return ResponseViewModel<Guid?>
                    .Failure(requestResult?.Message, (ResponseVmErrorCode?)requestResult?.requestErrorCode);
            }
            return ResponseViewModel<Guid?>
                .Success(requestResult.Data);
        }

        [HttpPatch]
        public async Task<ResponseViewModel<bool>> PublishQuiz(Guid quizId, CancellationToken ct)
        {
            var RequestResult = await _mediator
                .Send(new PublishQuizOrchestrator(quizId), ct);

            if (!RequestResult.IsSuccess)
            {
                return ResponseViewModel<bool>
                     .Failure(RequestResult.Message, (ResponseVmErrorCode?)RequestResult.requestErrorCode);
            }
            return ResponseViewModel<bool>
                 .Success(RequestResult.Data);
        }

        [HttpPatch]
        public async Task<ResponseViewModel<bool>> UnPublishQuiz(Guid quizId, CancellationToken ct)
        {
            var RequestResult = await _mediator
                .Send(new UnPublishQuizCommandRequest(quizId), ct);

            if (!RequestResult.IsSuccess)
            {
                return ResponseViewModel<bool>
                     .Failure(RequestResult.Message, (ResponseVmErrorCode?)RequestResult.requestErrorCode);
            }
            return ResponseViewModel<bool>
                 .Success(RequestResult.Data);
        }
    }
}
