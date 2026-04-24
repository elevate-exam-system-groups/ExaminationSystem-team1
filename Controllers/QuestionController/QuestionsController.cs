using Microsoft.AspNetCore.Authorization;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Add;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Update;
using ExaminationSystem.Controllers.QuestionController.Mapping;

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
        public async Task<ActionResult<ResponseViewModel<AddQuestionResponseVM>>> Create(
            Guid quizId, [FromBody] AddQuestionVM vm)
        {
            var command = vm.ToCommand(quizId);

            var result = await _mediator.Send(command);

            var mappedResult = result.IsSuccess
                ? RequestResult<AddQuestionResponseVM>.Success(result.Data.ToViewModel(), result.Message)
                : RequestResult<AddQuestionResponseVM>.Failure(result.Message, result.requestErrorCode);

            return HandleResult(mappedResult, 201);
        }

        [HttpPut("questions/{id:guid}")]
        public async Task<ActionResult<ResponseViewModel<UpdateQuestionResponseVM>>> Update(
            Guid id, [FromBody] UpdateQuestionVM vm)
        {
            var command = vm.ToCommand(id);

            var result = await _mediator.Send(command);

            var mappedResult = result.IsSuccess
                ? RequestResult<UpdateQuestionResponseVM>.Success(result.Data.ToViewModel(), result.Message)
                : RequestResult<UpdateQuestionResponseVM>.Failure(result.Message, result.requestErrorCode);

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
