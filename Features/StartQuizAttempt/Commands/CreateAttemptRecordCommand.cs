
namespace ExaminationSystem.Features.StartQuizAttempt.Commands
{
    public record CreateAttemptRecordCommand(Guid QuizId, string StudentId)
        : IRequest<RequestResult<Guid>>;

    public class CreateAttemptRecordCommandHandler
        : IRequestHandler<CreateAttemptRecordCommand, RequestResult<Guid>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptRepository;
        private readonly IMediator _mediator;

        public CreateAttemptRecordCommandHandler(IGeneralRepository<QuizAttempt> quizAttemptRepository, IMediator mediator)
        {
            _quizAttemptRepository = quizAttemptRepository;
            _mediator = mediator;
        }

        public async Task<RequestResult<Guid>> Handle(CreateAttemptRecordCommand request, CancellationToken cancellationToken)
        {
            var durationResult = await _mediator
                .Send(new GetQuizDurationInMinutesQuery(request.QuizId), cancellationToken);

            if (!durationResult.IsSuccess)
                return RequestResult<Guid>
                    .Failure(durationResult.Message, durationResult.requestErrorCode);

            Guid newAttemptId = _quizAttemptRepository.AddAndReturnId(new QuizAttempt
            {
                Id = Guid.NewGuid(),
                QuizId = request.QuizId,
                StudentId = request.StudentId,
                StartTime = DateTime.UtcNow,
                Status = QuizAttemptStatus.InProgress,
                DeadLine = DateTime.UtcNow.AddMinutes(durationResult.Data)
            });

            await _quizAttemptRepository.SaveChangesAsync();

            return RequestResult<Guid>.Success(newAttemptId);
        }
    }
}
