using AutoMapper;
using ExaminationSystem.Controllers.AttemptController.ViewModels;
using ExaminationSystem.Features.StartQuizAttempt.Orchestrators;
using ExaminationSystem.Features.SubmitAnswerAttempt.Orchestrators;
using ExaminationSystem.Features.SubmitQuizAttempt.Orchestrators;



namespace ExaminationSystem.Controllers.AttemptController
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AttemptController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AttemptController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ResponseViewModel<StartAttemptVM>> StartQuizAttempt(Guid QuizId, string StudentId, CancellationToken cancellationToken)
        {
            var result = await _mediator
                .Send(new StartQuizAttemptOrchestrator(QuizId, StudentId), cancellationToken);
            if (!result.IsSuccess)
            {
                return ResponseViewModel<StartAttemptVM>
                    .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode);
            }
            var vm = _mapper.Map<StartAttemptVM>(result.Data);
            return ResponseViewModel<StartAttemptVM>.Success(vm);

        }

        [HttpPost]
        public async Task<ResponseViewModel<bool>> AtemptAnswerQuestion(AttemptAnswerVM AnswerVM, CancellationToken cancellationToken)
        {
            var Request = await _mediator
                .Send(new SubmitAnswerOrchestrator(
                    AnswerVM.StudentId,
                    AnswerVM.AttemptId,
                    AnswerVM.QuestionId,
                    AnswerVM.OprionId),
                    cancellationToken);

            if (!Request.IsSuccess)
            {
                return ResponseViewModel<bool>
                    .Failure(Request.Message, (ResponseVmErrorCode?)Request.requestErrorCode);
            }

            return ResponseViewModel<bool>.Success(Request.Data);

        }

        [HttpPost]
        public async Task<ResponseViewModel<SubmitAttemptResponseVM>> SubmitQuizAttempt(Guid AttemptId, string StudentId, CancellationToken cancellationToken)
        {
            var result = await _mediator
                .Send(new SubmitQuizAttemptOrchestrator(StudentId, AttemptId), cancellationToken);

            if (!result.IsSuccess)
            {
                return ResponseViewModel<SubmitAttemptResponseVM>
                    .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode);
            }

            return ResponseViewModel<SubmitAttemptResponseVM>
                .Success(new SubmitAttemptResponseVM
                (
                    result.Data.Score,
                    result.Data.IsPassed,
                    result.Data.Status
                ));
        }
    }
}
