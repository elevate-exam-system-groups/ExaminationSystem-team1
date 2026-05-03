using ExaminationSystem.Features.DiplomaFeatures.DeleteDiploma.Commands;

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
                var result = await _mediator.Send(new DeleteDiplomaCommand(diplomaID));

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<bool>
                          .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode));
                }

                return Results.Ok(ResponseViewModel<bool>.Success(result.Data));
            })
            .WithTags("Diploma")
            .WithName("DeleteDiploma");
        }
    }
}
