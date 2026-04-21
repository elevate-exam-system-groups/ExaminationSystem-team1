namespace ExaminationSystem.Features.AnswerQuestion.Requests
{
    public record RecordAnswerCommandRequest(Guid QuestionId, Guid SelectedOptionId, Guid AttemptId)
        : IRequest<RequestResult<bool>>;

    public class RecordAnswerCommandRequestValidator : AbstractValidator<RecordAnswerCommandRequest>
    {
        public RecordAnswerCommandRequestValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("QuestionId is required");
            RuleFor(x => x.SelectedOptionId)
                .NotEmpty().WithMessage("SelectedOptionId is required");
            RuleFor(x => x.AttemptId)
                .NotEmpty().WithMessage("AttemptId is required");
        }
    }

    public class RecordAnswerCommandRequestHandler
        : IRequestHandler<RecordAnswerCommandRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<AttemptAnswer> _attemptAnswersRepository;
        private readonly IValidator<RecordAnswerCommandRequest> _validator;
        public RecordAnswerCommandRequestHandler(IGeneralRepository<AttemptAnswer> attemptAnswersRepository, IValidator<RecordAnswerCommandRequest> validator)
        {
            _attemptAnswersRepository = attemptAnswersRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(RecordAnswerCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string
                    .Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<bool>
                    .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            _attemptAnswersRepository.Add(new AttemptAnswer
            {
                Id = Guid.NewGuid(),
                QuestionId = request.QuestionId,
                SelectedOptionId = request.SelectedOptionId,
                QuizAttemptId = request.AttemptId
            });
            await _attemptAnswersRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true);
        }
    }

}
