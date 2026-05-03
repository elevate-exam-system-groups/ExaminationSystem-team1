using ExaminationSystem.Controllers.QuestionController.Mapping;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Add;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Delete;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Update;
using ExaminationSystem.Controllers.QuestionController.Mapping;
using ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Orchestrators;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Delete;
using Microsoft.AspNetCore.Authorization;

namespace ExaminationSystem.Controllers.QuestionController
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class QuestionsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public QuestionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("quizzes/{quizId:guid}/questions")]
        public async Task<ActionResult<ResponseViewModel<AddQuestionResponseVM>>> Create(
            Guid quizId,
            [FromBody] AddQuestionVM vm)
        {
            var command = vm.ToCommand(quizId);

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return HandleResult(RequestResult<AddQuestionResponseVM>.Failure(
                    result.Message, result.requestErrorCode));

            var viewModel = result.Data!.ToViewModel();
            return HandleResult(RequestResult<AddQuestionResponseVM>.Success(
                viewModel, result.Message), successStatusCode: 201);
        }

        [HttpPut("questions/{id:guid}")]
        public async Task<ActionResult<ResponseViewModel<UpdateQuestionResponseVM>>> Update(
            Guid id,
            [FromBody] UpdateQuestionVM vm)
        {
            var command = vm.ToCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return HandleResult(RequestResult<UpdateQuestionResponseVM>.Failure(
                    result.Message, result.requestErrorCode));

            var viewModel = result.Data!.ToViewModel();

            return HandleResult(RequestResult<UpdateQuestionResponseVM>.Success(
                viewModel, result.Message));
        }


        [HttpDelete("questions/{id:guid}")]
        public async Task<ActionResult<ResponseViewModel<DeleteQuestionResponseVM>>> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteQuestionOrchestrator(id));

            if (!result.IsSuccess)
                return HandleResult(RequestResult<DeleteQuestionResponseVM>.Failure(
                    result.Message, result.requestErrorCode));

            var viewModel = result.Data!.ToViewModel();
            return HandleResult(RequestResult<DeleteQuestionResponseVM>.Success(
                viewModel, result.Message));
        }

    }

}