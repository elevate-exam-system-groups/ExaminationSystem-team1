using ExaminationSystem.Features.Questions_OptionsModule.Common.Command.Request;
using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;
using ExaminationSystem.Features.Questions_OptionsModule.Common.Query;

namespace ExaminationSystem.Features.Questions_OptionsModule.Common.Command.Handler
{
    public class ValidateQuestionDeletionCommandHandler
      : IRequestHandler<ValidateQuestionDeletionCommand, RequestResult<QuestionDeletionValidationDto>>
    {

        private readonly IMediator _mediator;
        public ValidateQuestionDeletionCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResult<QuestionDeletionValidationDto>> Handle(
            ValidateQuestionDeletionCommand request, CancellationToken ct)
        {
            // 1️⃣ Get question info (includes published check)
            var questionInfoResult = await _mediator.Send(
                new GetQuestionInfoQuery(request.QuestionId), ct);

            if (!questionInfoResult.IsSuccess)
            {
                return RequestResult<QuestionDeletionValidationDto>.Failure(
                    questionInfoResult.Message,
                    questionInfoResult.requestErrorCode);
            }

            var questionInfo = questionInfoResult.Data;

            // 2️⃣ Check for active attempts
            var attemptsResult = await _mediator.Send(
                new CheckActiveAttemptsQuery(questionInfo.QuizId), ct);

            if (!attemptsResult.IsSuccess)
            {
                return RequestResult<QuestionDeletionValidationDto>.Failure(
                    attemptsResult.Message,
                    attemptsResult.requestErrorCode);
            }

            // 3️⃣ All validations passed
            var validationDto = new QuestionDeletionValidationDto(
                questionInfo.QuestionId,
                questionInfo.QuizId,
                true,
                null
            );

            return RequestResult<QuestionDeletionValidationDto>.Success(validationDto);
        }
    }
}
