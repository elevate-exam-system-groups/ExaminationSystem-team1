using ExaminationSystem.Features.QuizFeatures.PublishQuiz.Orchestrators;

namespace ExaminationSystem.Features.QuizFeatures.PublishQuiz
{
    public static class PublishQuizEndpoint
    {
        public static void MapPublishQuizEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/v2/Quiz/PublishQuiz", async (
                [FromQuery] Guid quizId,
                IMediator _mediator,
                CancellationToken ct) =>
            {
                var RequestResult = await _mediator
                    .Send(new PublishQuizOrchestrator(quizId), ct);

                if (!RequestResult.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<bool>
                         .Failure(RequestResult.Message, (ResponseVmErrorCode?)RequestResult.requestErrorCode));
                }

                return Results.Ok(ResponseViewModel<bool>.Success(RequestResult.Data));
            })
            .WithTags("Quiz")
            .WithName("PublishQuiz");
        }
    }
}
