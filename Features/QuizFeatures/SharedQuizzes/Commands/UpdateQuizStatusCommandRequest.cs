namespace ExaminationSystem.Features.QuizFeatures.SharedQuizzes.Commands
{
    public record UpdateQuizStatusCommandRequest(Guid QuizId, QuizStatus NewStatus) : IRequest<RequestResult<bool>>;

    public class UpdateQuizStatusCommandRequestValidator : AbstractValidator<UpdateQuizStatusCommandRequest>
    {
        public UpdateQuizStatusCommandRequestValidator()
        {
            RuleFor(r => r.QuizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");

            RuleFor(r => r.NewStatus)
                .IsInEnum()
                .WithMessage("Invalid quiz status.");
        }
    }

    public class UpdateQuizStatusCommandRequestHandler : IRequestHandler<UpdateQuizStatusCommandRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Quiz> _quizzesRepository;
        private readonly IValidator<UpdateQuizStatusCommandRequest> _validator;

        public UpdateQuizStatusCommandRequestHandler(IGeneralRepository<Quiz> quizzesRepository, IValidator<UpdateQuizStatusCommandRequest> validator)
        {
            _quizzesRepository = quizzesRepository;
            _validator = validator;
        }

        public async Task<RequestResult<bool>> Handle(UpdateQuizStatusCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
              .ValidateRequestAsync<UpdateQuizStatusCommandRequest, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var quizId = _quizzesRepository.GetById(request.QuizId)
                            .Select(q => q.Id)
                            .FirstOrDefault();

            if (quizId == Guid.Empty)
                return RequestResult<bool>.Failure("Quiz not found", RequestErrorCode.NotFound);

            var quiz = new Quiz
            {
                Id = quizId,
                Status = request.NewStatus
            };

            _quizzesRepository.UpdateInclude(quiz, nameof(Quiz.Status));
            // await _quizRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true);
        }
    }
}
