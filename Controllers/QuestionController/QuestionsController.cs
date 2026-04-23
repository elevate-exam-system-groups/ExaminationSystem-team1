using Microsoft.AspNetCore.Authorization;
using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Orchestrator;

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
        public async Task<ActionResult<ResponseViewModel<AddQuestionResponseVM>>> Create
        (Guid quizId, [FromBody] AddQuestionVM vm)
        {

            var command = new AddQuestionOrchestratorCommand
            {
                QuizId = quizId,
                Text = vm.Text,
                Explanation = vm.Explanation,
                Options = vm.Options.Select(o => new OptionDto(o.Text, o.IsCorrect)).ToList()
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return StatusCode(
                    GetStatusCode(result.requestErrorCode),
                    ResponseViewModel<AddQuestionResponseVM>.Failure(
                        result.Message, MapErrorCode(result.requestErrorCode))
                );
            }

            return StatusCode(201, ResponseViewModel<AddQuestionResponseVM>
                .Success(new AddQuestionResponseVM(true), result.Message));
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

            if (!result.IsSuccess)
            {
                return StatusCode(
                    GetStatusCode(result.requestErrorCode),
                    ResponseViewModel<UpdateQuestionResponseVM>.Failure(
                        result.Message, MapErrorCode(result.requestErrorCode))
                );
            }

            return Ok(ResponseViewModel<UpdateQuestionResponseVM>.Success(
                new UpdateQuestionResponseVM(true),
                result.Message));
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