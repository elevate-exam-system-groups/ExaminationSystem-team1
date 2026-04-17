using ExaminationSystem.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.CreateQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.CreateQuestion.DTOs;
using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.DTOs;
using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Controllers.QuestionController
{
    [Route("api/admin/quizzes/{quizId}/questions")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class QuestionsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public QuestionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseViewModel<AddQuestionResponse>>> Create(
                      Guid quizId, [FromBody] AddQuestionViewModel vm)
        {
            if (quizId != vm.QuizId)
                return BadRequest(ResponseViewModel<AddQuestionResponse>.Failure("Quiz ID mismatch"));

            var command = new AddQuestionCommand
            {
                QuizId = vm.QuizId,
                Text = vm.Text,
                Explanation = vm.Explanation,
                Options = vm.Options.Select(o => new OptionDto(o.Text, o.IsCorrect)).ToList()
            };

            var result = await _mediator.Send(command);

            return HandleResult(result, successStatusCode: 201);
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<ResponseViewModel<UpdateQuestionResponse>>> Update(
            Guid id,  [FromBody] UpdateQuestionViewModel vm)
        {
            if (id != vm.Id)
                return BadRequest(ResponseViewModel<UpdateQuestionResponse>.Failure("Question ID mismatch"));

            var command = new UpdateQuestionOptionsOrchestrator
            {
                Id = vm.Id,
                Text = vm.Text,
                Explanation = vm.Explanation,
                Options = vm.Options.Select(o => new UpdateOptionDto(o.Id, o.Text, o.IsCorrect)).ToList()
            };

            var result = await _mediator.Send(command);

            return HandleResult(result);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<ResponseViewModel<DeleteOptionsResponse>>> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteQuestionOrchestrator(id));

            return HandleResult(result);
        }



    }
}