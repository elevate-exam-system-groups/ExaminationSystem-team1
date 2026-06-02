using ExaminationSystem.Controllers.DiplomaController.ViewModels;
using ExaminationSystem.Features.DiplomaFeatures.CreateDiploma.Commands;

namespace ExaminationSystem.Features.DiplomaFeatures.CreateDiploma
{
    public static class CreateDiplomaEndpoint
    {
        public static void MapCreateDiplomaEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/CreateDiploma", async (
                [FromBody] CreateDiplomaRequestVM request,
                IMediator _mediator) =>
            {
                var result = await _mediator
                    .Send(new CreateDiplomaCommandRequest(request.Title, request.Description));

                if (!result.IsSuccess)
                {
                    //tstst
                    return Results.BadRequest(ResponseViewModel<CreateDiplomaResponseVM>
                          .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode));
                }

                var responseVM = new CreateDiplomaResponseVM(
                    result.Data.Id,
                    result.Data.Title,
                    result.Data.Description,
                    result.Data.Status);

                return Results.Ok(ResponseViewModel<CreateDiplomaResponseVM>.Success(responseVM));
            })
            .WithTags("Diploma")
            .WithName("CreateDiploma");
        }
    }
}
