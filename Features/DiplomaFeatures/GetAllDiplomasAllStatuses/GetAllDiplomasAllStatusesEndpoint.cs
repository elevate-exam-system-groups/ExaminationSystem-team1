using ExaminationSystem.Controllers.DiplomaController.ViewModels;
using ExaminationSystem.Features.DiplomaFeatures.GetAllDiplomasAllStatuses.Queries;

namespace ExaminationSystem.Features.DiplomaFeatures.GetAllDiplomasAllStatuses
{
    public static class GetAllDiplomasAllStatusesEndpoint
    {
        public static void MapGetAllDiplomasAllStatusesEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v2/Diploma/GetAllDiplomasAllStatuses", async (
                [AsParameters] AllDiplomasPaginatedRequestVM requestVM,
                IMediator mediator) =>
            {
                var result = await mediator.Send(
                    new GetAllDiplomasAllStatusesQuery(requestVM.Page, requestVM.PerPage));

                if (!result.IsSuccess)
                {
                    var failureResponse = ResponseViewModel<GetAllDiplomasPaginatedResponseVM>
                        .Failure(result.Message ?? "Request Failed", (ResponseVmErrorCode?)result.requestErrorCode);

                    return Results.BadRequest(failureResponse);
                }

                var responseVM = new GetAllDiplomasPaginatedResponseVM(
                    result.Data.Data,
                    result.Data.Page,
                    result.Data.PerPage,
                    result.Data.Total,
                    result.Data.TotalPages);

                var finalResponse = ResponseViewModel<GetAllDiplomasPaginatedResponseVM>.Success(responseVM);

                return Results.Ok(finalResponse);
            })
            .WithTags("Diploma")
            .WithName("GetAllDiplomasAllStatuses");
        }
    }
}
