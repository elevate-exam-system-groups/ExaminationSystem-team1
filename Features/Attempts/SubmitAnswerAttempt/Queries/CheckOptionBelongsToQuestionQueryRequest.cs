namespace ExaminationSystem.Features.Attempts.SubmitAnswerAttempt.Queries
{
    public record CheckOptionBelongsToQuestionQueryRequest(Guid optionId, Guid questionId)
        : IRequest<RequestResult<bool>>;

    public class CheckOptionBelongsToQuestionQueryRequestValidator
    : AbstractValidator<CheckOptionBelongsToQuestionQueryRequest>
    {
        public CheckOptionBelongsToQuestionQueryRequestValidator()
        {
            RuleFor(x => x.optionId)
                .NotEmpty().WithMessage("Option ID is required");
            RuleFor(x => x.questionId)
                .NotEmpty().WithMessage("Question ID is required");
        }
    }

    public class CheckOptionBelongsToQuestionQueryRequestHandler
        : IRequestHandler<CheckOptionBelongsToQuestionQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Option> _optionsRepository;
        private readonly IValidator<CheckOptionBelongsToQuestionQueryRequest> _validator;
        public CheckOptionBelongsToQuestionQueryRequestHandler(IGeneralRepository<Option> optionsRepository, IValidator<CheckOptionBelongsToQuestionQueryRequest> validator)
        {
            _optionsRepository = optionsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckOptionBelongsToQuestionQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                var result = RequestResult<bool>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }
            var isOptionPartOfQuestion = _optionsRepository
               .Get(o => o.Id == request.optionId && o.QuestionId == request.questionId)
               .Any();
            if (!isOptionPartOfQuestion)
            {
                return RequestResult<bool>
                    .Failure("Option is not part of the question", RequestErrorCode.UnprocessableEntity);
            }
            return RequestResult<bool>.Success(isOptionPartOfQuestion);
        }
    }

}
