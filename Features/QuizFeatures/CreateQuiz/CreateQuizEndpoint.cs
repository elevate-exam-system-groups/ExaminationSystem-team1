using ExaminationSystem.Controllers.QuizController.ViewModels;
using ExaminationSystem.Features.QuizFeatures.CreateQuiz.Commands;

namespace ExaminationSystem.Features.QuizFeatures.CreateQuiz
{
    public static class CreateQuizEndpoint
    {
        public static void MapCreateQuizEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v2/Quiz/CreateQuiz", async (
                [FromBody] CreateQuizRequestVM NewQuizVM,
                IMediator _mediator,
                CancellationToken ct) =>
            {
                var requestResult = await _mediator
                    .Send(new CreateQuizCommand(
                        NewQuizVM.Title,
                        NewQuizVM.DiplomaId,
                        NewQuizVM.DurationInMinutes,
                        NewQuizVM.PassScore,
                        NewQuizVM.MaxAttempts,
                        NewQuizVM.Instructions),
                    ct);

                if (!requestResult.IsSuccess)
                {

                    /////tst
                    return Results.BadRequest(ResponseViewModel<Guid?>
                        .Failure(requestResult?.Message, (ResponseVmErrorCode?)requestResult?.requestErrorCode));
                }

                return Results.Ok(ResponseViewModel<Guid?>.Success(requestResult.Data));
            })
            .WithTags("Quiz")
            .WithName("CreateQuiz");
        }
    }
}
