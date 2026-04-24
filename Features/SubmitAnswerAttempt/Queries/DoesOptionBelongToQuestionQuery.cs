
namespace ExaminationSystem.Features.SubmitAnswerAttempt.Queries
{
    public record DoesOptionBelongToQuestionQuery(Guid optionId, Guid questionId)
        : IRequest<RequestResult<bool>>;


    public class DoesOptionBelongToQuestionQueryValidator
    : AbstractValidator<DoesOptionBelongToQuestionQuery>
    {
        public DoesOptionBelongToQuestionQueryValidator()
        {
            RuleFor(x => x.optionId)
                .NotEmpty().WithMessage("Option ID is required");
            RuleFor(x => x.questionId)
                .NotEmpty().WithMessage("Question ID is required");
        }
    }


    public class DoesOptionBelongToQuestionQueryHandler
        : IRequestHandler<DoesOptionBelongToQuestionQuery, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Option> _optionsRepository;
        private readonly IValidator<DoesOptionBelongToQuestionQuery> _validator;
        public DoesOptionBelongToQuestionQueryHandler(IGeneralRepository<Option> optionsRepository, IValidator<DoesOptionBelongToQuestionQuery> validator)
        {
            _optionsRepository = optionsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(DoesOptionBelongToQuestionQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
              .ValidateRequestAsync<DoesOptionBelongToQuestionQuery, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var isOptionPartOfQuestion = await _optionsRepository
               .Get(o => o.Id == request.optionId && o.QuestionId == request.questionId)
               .AnyAsync(cancellationToken);

            if (!isOptionPartOfQuestion)
            {
                return RequestResult<bool>
                    .Failure("Option is not part of the question", RequestErrorCode.UnprocessableEntity);
            }

            return RequestResult<bool>.Success(isOptionPartOfQuestion);
        }
    }

}
