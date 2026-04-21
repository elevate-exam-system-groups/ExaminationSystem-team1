namespace ExaminationSystem.Features.QuizModule.SubmitAnswerAttempt.Requests
{
    public record AutoSubmitAttemptCommandRequest(Guid AttemptId) : IRequest<RequestResult<bool>>;

    public class AutoSubmitAttemptCommandRequestValidator : AbstractValidator<AutoSubmitAttemptCommandRequest>
    {
        public AutoSubmitAttemptCommandRequestValidator()
        {
            RuleFor(x => x.AttemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
        }
    }

    public class AutoSubmitAttemptCommandRequestHandler
        : IRequestHandler<AutoSubmitAttemptCommandRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<AutoSubmitAttemptCommandRequest> _validator;
        public AutoSubmitAttemptCommandRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<AutoSubmitAttemptCommandRequest> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(AutoSubmitAttemptCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<bool>
                      .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            var attemptData = _quizAttemptsRepository
            .Get(qa => qa.Id == request.AttemptId)
            .Select(qa => new
            {
                TotalQuestions = qa.Quiz.Questions.Count,
                CorrectAnswers = qa.UserAnswers
                 .Count(a => a.SelectedOption.IsCorrect),
                qa.Quiz.PassScore,
                qa.StudentId
            }).FirstOrDefault();

            throw new NotImplementedException();
        }
    }
}
