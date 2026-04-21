using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands.DeleteOptions;
using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands.DeleteQuestionOnly;
using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Queries.CheckActiveAttempts;
using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Queries.GetQuestionInfo;

namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion
{
    public class DeleteQuestionOrchestratorHandler
        : IRequestHandler<DeleteQuestionOrchestrator, RequestResult<DeleteResponse>>
    {

        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteQuestionOrchestratorHandler(
            IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResult<DeleteResponse>> Handle(
            DeleteQuestionOrchestrator request, CancellationToken ct)
        {

            var questionInfo = await _mediator.Send(
                 new GetQuestionInfoQuery(request.Id), ct);

            if (!questionInfo.IsSuccess)
                return RequestResult<DeleteResponse>.Failure(
                    questionInfo.Message,
                    questionInfo.requestErrorCode);

            var result = questionInfo.Data;

            if (result.QuizStatus == QuizStatus.Published)
            {
                return RequestResult<DeleteResponse>.Failure(
                    "Cannot delete question from a published quiz. Unpublish quiz first.",
                    RequestErrorCode.Conflict);
            }

            var checkAttemptsResult = await _mediator.Send(
                new CheckActiveAttemptsQuery(result.QuizId), ct);
            if (!checkAttemptsResult.IsSuccess)
                return RequestResult<DeleteResponse>.Failure(
                      "Cannot delete question while there are active quiz attempts.",
                    RequestErrorCode.Conflict);


            // Delete options
            var deleteOptionsResult = await _mediator.Send(
                new DeleteOptionsCommand(request.Id), ct);

            if (!deleteOptionsResult.IsSuccess)
            {
                return RequestResult<DeleteResponse>.Failure(
                    deleteOptionsResult.Message,
                    deleteOptionsResult.requestErrorCode);
            }

            // Delete question
            var deleteQuestionResult = await _mediator.Send(
                new DeleteQuestionOnlyCommand(request.Id), ct);

            if (!deleteQuestionResult.IsSuccess)
            {
                return RequestResult<DeleteResponse>.Failure(
                    deleteQuestionResult.Message,
                    deleteQuestionResult.requestErrorCode);
            }

            await _unitOfWork.SaveChangesAsync(); 

            return RequestResult<DeleteResponse>.Success(
                new DeleteResponse(true),
                "Question and all options deleted successfully");
        }

    }
}
