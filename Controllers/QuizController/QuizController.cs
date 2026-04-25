
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

        [HttpPatch]
        public async Task<ResponseViewModel<bool>> PublishQuiz(Guid quizId)
        {
            var RequestResult = await _mediator
                .Send(new PublishQuizOrchestrator(quizId));

            if (!RequestResult.IsSuccess)
            {
                return ResponseViewModel<bool>
                     .Failure(RequestResult.Message, (ResponseVmErrorCode?)RequestResult.requestErrorCode);
            }
            return ResponseViewModel<bool>
                 .Success(RequestResult.Data);
        }

        [HttpPatch]
        public async Task<ResponseViewModel<bool>> UnPublishQuiz(Guid quizId)
        {
            var RequestResult = await _mediator
                .Send(new UnPublishQuizCommandRequest(quizId));

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
