using Microsoft.AspNetCore.Authorization;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Add;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Update;
using ExaminationSystem.Controllers.QuestionController.Mapping;

namespace ExaminationSystem.Controllers.QuestionController
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class QuestionsController : BaseApiController
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

 
    }
}
