using ExaminationSystem.Controllers.DiplomaController.ViewModels;
using ExaminationSystem.Features.DiplomaFeatures.UpdateDiploma.Commands;

namespace ExaminationSystem.Features.DiplomaFeatures.UpdateDiploma
{
    public static class UpdateDiplomaEndpoint
    {
        public static void MapUpdateDiplomaEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/v2/Diploma/UpdateDiploma", async (
                [FromBody] UpdateDiplomaRequestVM request,
                IMediator _mediator) =>
            {
                var result = await _mediator
                     .Send(new UpdateDiplomaCommandRequest(request.Id, request.Title, request.Description));

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<UpdateDiplomaResponseVM>
                          .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode));
                }

                var responseVM = new UpdateDiplomaResponseVM(result.Data.Id, result.Data.Title, result.Data.Description, result.Data.Status);
                
                return Results.Ok(ResponseViewModel<UpdateDiplomaResponseVM>.Success(responseVM));
            })
            .WithTags("Diploma")
            .WithName("UpdateDiploma");
        }
    }
}
