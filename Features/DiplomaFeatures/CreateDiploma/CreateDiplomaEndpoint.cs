using ExaminationSystem.Controllers.DiplomaController.ViewModels;
using ExaminationSystem.Features.DiplomaFeatures.CreateDiploma.Publishers;

namespace ExaminationSystem.Features.DiplomaFeatures.CreateDiploma
{
    public static class CreateDiplomaEndpoint
    {
        public static void MapCreateDiplomaEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v2/Diploma/CreateDiploma", async (
                [FromBody] CreateDiplomaRequestVM request,
                IMediator _mediator) =>
            {
                var result = await _mediator
                  .Send(new PublishDiplomaCommandRequest(request.Title, request.Description));

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<string>.Failure(result.Message));
                }

                return Results.Accepted(value: "Request is being processed.");
            })
                .WithTags("Diploma")
                .WithName("CreateDiploma");
        }
    }
}
