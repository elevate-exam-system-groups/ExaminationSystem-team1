using ExaminationSystem.Features.Common.FeatureExtensions;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Common.AttemptRequests.Commands
{
    public record UpdateQuizAttemptStatusCommand(Guid AttemptId, QuizAttemptStatus Status)
        : IRequest<RequestResult<bool>>;

    public class UpdateQuizAttemptStatusCommandValidator : AbstractValidator<UpdateQuizAttemptStatusCommand>
    {
        public UpdateQuizAttemptStatusCommandValidator()
        {
            RuleFor(x => x.AttemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value");
        }
    }

    public class UpdateQuizAttemptStatusCommandHandler
        : IRequestHandler<UpdateQuizAttemptStatusCommand, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<UpdateQuizAttemptStatusCommand> _validator;
        public UpdateQuizAttemptStatusCommandHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<UpdateQuizAttemptStatusCommand> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(UpdateQuizAttemptStatusCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                 .ValidateRequestAsync<UpdateQuizAttemptStatusCommand, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;


            var attemptExists = await _quizAttemptsRepository
                .Get(qa => qa.Id == request.AttemptId)
                .AnyAsync();

            if (!attemptExists)
            {
                return RequestResult<bool>.Failure("Quiz attemptExists not found.", RequestErrorCode.NotFound);
            }

            var attempt = new QuizAttempt
            {
                Id = request.AttemptId,
                Status = request.Status,
                SubmittedAt = request.Status == QuizAttemptStatus.Submitted ? DateTime.UtcNow : null
            };

            _quizAttemptsRepository.UpdateInclude(attempt, nameof(QuizAttempt.Status), nameof(QuizAttempt.SubmittedAt));

            await _quizAttemptsRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true);
        }
    }
}