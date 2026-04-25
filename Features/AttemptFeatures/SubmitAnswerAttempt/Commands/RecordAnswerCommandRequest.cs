using ExaminationSystem.Features.Common.FeatureExtensions;

namespace ExaminationSystem.Features.AttemptFeatures.SubmitAnswerAttempt.Commands
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
            var validationResult = await _validator
                     .ValidateRequestAsync<RecordAnswerCommandRequest, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;


            var AttemptAnswerId = _attemptAnswersRepository
             .Get(a => a.QuizAttemptId == request.AttemptId &&
              a.QuestionId == request.QuestionId)
             .Select(a => a.Id)
             .FirstOrDefault();


            if (AttemptAnswerId != Guid.Empty)
            {
                var existingAnswer = new AttemptAnswer
                {
                    QuestionId = request.QuestionId,
                    Id = AttemptAnswerId,
                    SelectedOptionId = request.SelectedOptionId
                };

                _attemptAnswersRepository.UpdateInclude(existingAnswer, nameof(existingAnswer.SelectedOptionId));
                await _attemptAnswersRepository.SaveChangesAsync();
                return RequestResult<bool>.Success(true);
            }

            _attemptAnswersRepository.Add(new AttemptAnswer
            {
                Id = Guid.NewGuid(),
                QuestionId = request.QuestionId,
                SelectedOptionId = request.SelectedOptionId,
                QuizAttemptId = request.AttemptId,
                AnsweredAt = DateTime.UtcNow
            });

            await _attemptAnswersRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true);
        }
    }

}
