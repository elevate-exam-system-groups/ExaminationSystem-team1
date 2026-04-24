using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands.DeleteOptions;
using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands.DeleteQuestionOnly;
using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Queries.CheckActiveAttempts;
using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Queries.GetQuestionInfo;

namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Orchestrators
{
    public class DeleteQuestionOrchestratorHandler
        : IRequestHandler<DeleteQuestionOrchestrator, RequestResult<DeleteResponseDto>>
    {

        private readonly IMediator _mediator;
        public DeleteQuestionOrchestratorHandler(IMediator mediator)
         => _mediator = mediator;

        public async Task<RequestResult<DeleteResponseDto>> Handle(
            DeleteQuestionOrchestrator request, CancellationToken ct)
        {

            if (request.Id == Guid.Empty)
                return RequestResult<DeleteResponseDto>.Failure(
                    "Invalid Question ID", RequestErrorCode.ValidationError);


            var questionInfo = await _mediator.Send(
                 new GetQuestionInfoQuery(request.Id), ct);

            if (!questionInfo.IsSuccess)
                return RequestResult<DeleteResponseDto>.Failure(
                    questionInfo.Message,
                    questionInfo.requestErrorCode);

            var result = questionInfo.Data;

            if (result.QuizStatus == QuizStatus.Published)
            {
                return RequestResult<DeleteResponseDto>.Failure(
                    "Cannot delete question from a published quiz. Unpublish quiz first.",
                    RequestErrorCode.Conflict);
            }

            var checkAttemptsResult = await _mediator.Send(
                new CheckActiveAttemptsQuery(result.QuizId), ct);

            if (!checkAttemptsResult.IsSuccess)
                return RequestResult<DeleteResponseDto>.Failure(
                    checkAttemptsResult.Message,
                    checkAttemptsResult.requestErrorCode);

            if (checkAttemptsResult.Data.HasActiveAttempts)
            {
                return RequestResult<DeleteResponseDto>.Failure(
                    $"Cannot delete: {checkAttemptsResult.Data.ActiveAttemptsCount} " +
                    $"student(s) are currently taking this quiz.",
                    RequestErrorCode.Conflict);
            }


            var deleteOptionsResult = await _mediator.Send(
                new DeleteOptionsCommand(request.Id), ct);

            if (!deleteOptionsResult.IsSuccess)
            {
                return RequestResult<DeleteResponseDto>.Failure(
                    deleteOptionsResult.Message,
                    deleteOptionsResult.requestErrorCode);
            }


            var deleteQuestionResult = await _mediator.Send(
                new DeleteQuestionOnlyCommand(request.Id), ct);

            if (!deleteQuestionResult.IsSuccess)
            {
                return RequestResult<DeleteResponseDto>.Failure(
                    deleteQuestionResult.Message,
                    deleteQuestionResult.requestErrorCode);
            }


            return RequestResult<DeleteResponseDto>.Success(
                new DeleteResponseDto(true),
                "Question and all options deleted successfully");
        }



    }
}
