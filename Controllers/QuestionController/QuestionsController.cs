using Microsoft.AspNetCore.Authorization;
using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Orchestrator;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Add;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Update;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Delete;
using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Orchestrator;

namespace ExaminationSystem.Controllers.QuestionController
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class QuestionsController : ControllerBase
    {

        private readonly IMediator _mediator;
        public QuestionsController(IMediator mediator) => _mediator = mediator;

       
        [HttpPost("quizzes/{quizId:guid}/questions")]
        public async Task<ActionResult<ResponseViewModel<AddQuestionResponseVM>>> Create(
            Guid quizId, [FromBody] AddQuestionVM vm)
        {
            var command = new AddQuestionOrchestratorCommand
            {
                QuizId = quizId,
                Text = vm.Text,
                Explanation = vm.Explanation,
                Options = vm.Options.Select(o => new OptionDto(o.Text, o.IsCorrect)).ToList()
            };

            var result = await _mediator.Send(command);

            var mappedResult = result.IsSuccess
                ? RequestResult<AddQuestionResponseVM>.Success(
                    new AddQuestionResponseVM(result.Data.added), result.Message)
                : RequestResult<AddQuestionResponseVM>.Failure(
                    result.Message, result.requestErrorCode);

            return HandleResult(mappedResult, 201);
        }


        [HttpPut("questions/{id:guid}")]
        public async Task<ActionResult<ResponseViewModel<UpdateQuestionResponseVM>>> Update(
            Guid id, [FromBody] UpdateQuestionVM vm)
        {
            var command = new UpdateQuestionOptionsOrchestrator
            {
                Id = id,
                Text = vm.Text,
                Explanation = vm.Explanation,
                Options = vm.Options.Select(o => new UpdateOptionDto(o.Id, o.Text, o.IsCorrect)).ToList()
            };

            var result = await _mediator.Send(command);

            var mappedResult = result.IsSuccess
                ? RequestResult<UpdateQuestionResponseVM>.Success(
                    new UpdateQuestionResponseVM(result.Data.updated), result.Message)
                : RequestResult<UpdateQuestionResponseVM>.Failure(
                    result.Message, result.requestErrorCode);

            return HandleResult(mappedResult);
        }


        [HttpDelete("questions/{id:guid}")]
        public async Task<ActionResult<ResponseViewModel<DeleteQuestionResponseVM>>> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteQuestionOrchestrator(id));

            var mappedResult = result.IsSuccess
                ? RequestResult<DeleteQuestionResponseVM>.Success(
                    new DeleteQuestionResponseVM(result.Data.Deleted), result.Message)
                : RequestResult<DeleteQuestionResponseVM>.Failure(
                    result.Message, result.requestErrorCode);

            return HandleResult(mappedResult);
        }


        // Helper Methods (Common Logic) ---

        private ActionResult<ResponseViewModel<T>> HandleResult<T>(
            RequestResult<T> result, int successStatusCode = 200)
        {
            if (result.IsSuccess)
            {
                return StatusCode(successStatusCode, ResponseViewModel<T>.Success(
                    result.Data!, result.Message));
            }

            var statusCode = GetStatusCode(result.requestErrorCode);
            return StatusCode(statusCode, ResponseViewModel<T>.Failure(
                result.Message, MapErrorCode(result.requestErrorCode)));
        }

        private static ResponseVmErrorCode MapErrorCode(RequestErrorCode? code) => code switch
        {
            RequestErrorCode.NotFound => ResponseVmErrorCode.NotFound,
            RequestErrorCode.Conflict => ResponseVmErrorCode.Conflict,
            RequestErrorCode.ValidationError => ResponseVmErrorCode.ValidationError,
            _ => ResponseVmErrorCode.InternalServerError
        };

        private static int GetStatusCode(RequestErrorCode? code) => code switch
        {
            RequestErrorCode.NotFound => 404,
            RequestErrorCode.Conflict => 409,
            RequestErrorCode.ValidationError => 422,
            _ => 500
        };
    }
}