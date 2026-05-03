using ExaminationSystem.Features.QuizFeatures.UnpublishQuiz.Orchestrators;

namespace ExaminationSystem.Features.QuizFeatures.UnpublishQuiz
{
    public static class UnPublishQuizEndpoint
    {
        public static void MapUnPublishQuizEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/v2/Quiz/UnPublishQuiz", async (
                [FromQuery] Guid quizId,
                IMediator _mediator,
                CancellationToken ct) =>
            {
                var RequestResult = await _mediator
                    .Send(new UnPublishQuizCommandRequest(quizId), ct);

                if (!RequestResult.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<bool>
                         .Failure(RequestResult.Message, (ResponseVmErrorCode?)RequestResult.requestErrorCode));
                }

                return Results.Ok(ResponseViewModel<bool>.Success(RequestResult.Data));
            })
            .WithTags("Quiz")
            .WithName("UnPublishQuiz");
        }
    }
}
