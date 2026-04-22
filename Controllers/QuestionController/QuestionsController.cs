using Microsoft.AspNetCore.Authorization;
using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion;

namespace ExaminationSystem.Controllers.QuestionController
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class QuestionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuestionsController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost("quizzes/{quizId:guid}/questions")]
        public async Task<ActionResult<ResponseViewModel<AddQuestionResponse>>> Create(
            Guid quizId,
            [FromBody] AddQuestionViewModel vm)
        {
            var command = new AddQuestionOrchestratorCommand
            {
                QuizId = quizId,
                Text = vm.Text,
                Explanation = vm.Explanation,
                Options = vm.Options.Select(o => new OptionDto(o.Text, o.IsCorrect)).ToList()
            };

            var result = await _mediator.Send(command);
            return HandleResult(result, 201);
        }

        [HttpPut("questions/{id:guid}")]
        public async Task<ActionResult<ResponseViewModel<UpdateQuestionResponse>>> Update(
            Guid id,
            [FromBody] UpdateQuestionViewModel vm)
        {
            var command = new UpdateQuestionOptionsOrchestrator
            {
                Id = id,
                Text = vm.Text,
                Explanation = vm.Explanation,
                Options = vm.Options.Select(o => new UpdateOptionDto(o.Id, o.Text, o.IsCorrect)).ToList()
            };

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("questions/{id:guid}")]
        public async Task<ActionResult<ResponseViewModel<DeleteResponse>>> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteQuestionOrchestrator(id));
            return HandleResult(result);
        }

        private ActionResult<ResponseViewModel<T>> HandleResult<T>(
            RequestResult<T> result,
            int successStatusCode = 200)
        {
            if (result.IsSuccess)
            {
                var response = ResponseViewModel<T>.Success(result.Data!, result.Message);
                return StatusCode(successStatusCode, response);
            }

            var errorResponse = ResponseViewModel<T>.Failure(
                result.Message,
                MapErrorCode(result.requestErrorCode));

            var statusCode = GetStatusCode(result.requestErrorCode);
            return StatusCode(statusCode, errorResponse);
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