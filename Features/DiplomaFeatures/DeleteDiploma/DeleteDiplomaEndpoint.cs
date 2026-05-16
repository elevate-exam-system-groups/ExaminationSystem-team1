using ExaminationSystem.Features.DiplomaFeatures.DeleteDiploma.Senders;

namespace ExaminationSystem.Features.DiplomaFeatures.DeleteDiploma
{
    public static class DeleteDiplomaEndpoint
    {
        public static void MapDeleteDiplomaEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/v2/Diploma/DeleteDiploma", async (
                [FromQuery] Guid diplomaID,
                IMediator _mediator) =>
            {
                var result = await _mediator.Send(new SendDeleteDiplomaCommandRequest(diplomaID));

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<string>
                          .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode));
                }

                return Results.Accepted(value: "Delete command is being processed.");
            })
            .WithTags("Diploma")
            .WithName("DeleteDiploma");
        }
    }
}
