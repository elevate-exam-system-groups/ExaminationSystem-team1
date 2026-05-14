using ExaminationSystem.Features.DiplomaFeatures.DeleteDiploma.Senders;

namespace ExaminationSystem.Features.DiplomaFeatures.DeleteDiploma
{
    public static class DeleteDiplomaAsyncEndpoint
    {
        public static void MapDeleteDiplomaAsyncEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/v2/Diploma/DeleteDiplomaAsync", async (
                [FromQuery] Guid diplomaID,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new SendDeleteDiplomaCommandRequest(diplomaID));

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<string>
                        .Failure(result.Message ?? "Request Failed", (ResponseVmErrorCode?)result.requestErrorCode));
                }

                return Results.Accepted(value: "Delete command is being processed.");
            })
            .WithTags("Diploma")
            .WithName("DeleteDiplomaAsync");
        }
    }
}
