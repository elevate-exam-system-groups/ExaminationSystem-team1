using ExaminationSystem.Features.AttemptFeatures.StartQuizAttempt.Commands.CreateAttempt.DTOs;

namespace ExaminationSystem.Features.AttemptFeatures.StartQuizAttempt.Commands.CreateAttempt
{
    public record CreateAttemptRecordCommand(Guid QuizId, string StudentId)
        : IRequest<RequestResult<CreatedAttemptDTO>>;

    public class CreateAttemptRecordCommandHandler
        : IRequestHandler<CreateAttemptRecordCommand, RequestResult<CreatedAttemptDTO>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptRepository;
        private readonly IMediator _mediator;

        public CreateAttemptRecordCommandHandler(IGeneralRepository<QuizAttempt> quizAttemptRepository, IMediator mediator)
        {
            _quizAttemptRepository = quizAttemptRepository;
            _mediator = mediator;
        }

        public async Task<RequestResult<CreatedAttemptDTO>> Handle(CreateAttemptRecordCommand request, CancellationToken cancellationToken)
        {
            var durationResult = await _mediator
                .Send(new GetQuizDurationInMinutesQuery(request.QuizId), cancellationToken);

            if (!durationResult.IsSuccess)
                return RequestResult<CreatedAttemptDTO>
                    .Failure(durationResult.Message, durationResult.requestErrorCode);

            var deadline = DateTime.UtcNow.AddMinutes(durationResult.Data);

            Guid newAttemptId = _quizAttemptRepository.AddAndReturnId(new QuizAttempt
            {
                Id = Guid.NewGuid(),
                QuizId = request.QuizId,
                StudentId = request.StudentId,
                StartTime = DateTime.UtcNow,
                Status = QuizAttemptStatus.InProgress,
                DeadLine = deadline
            });

            await _quizAttemptRepository.SaveChangesAsync();

            return RequestResult<CreatedAttemptDTO>
                .Success(new CreatedAttemptDTO(newAttemptId, deadline));
        }
    }
}
