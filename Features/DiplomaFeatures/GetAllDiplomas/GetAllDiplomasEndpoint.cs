using ExaminationSystem.Controllers.DiplomaController.ViewModels;
using ExaminationSystem.Features.DiplomaFeatures.GetAllDiplomas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.DiplomaFeatures.GetAllDiplomas
{
    public static class GetAllDiplomasEndpoint
    {
        public static void MapGetAllDiplomasEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/GetAllDiplomas", async (
                [AsParameters] AllDiplomasPaginatedRequestVM requestVM, 
                IMediator _mediator) =>
            {
                var result = await _mediator.Send(new GetAllDiplomasQuery(requestVM.Page, requestVM.PerPage));

                if (!result.IsSuccess)
                {
                    var failureResponse = ResponseViewModel<GetAllDiplomasPaginatedResponseVM>
                        .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode);

                    return Results.BadRequest(failureResponse);
                }

                var responseVM = new GetAllDiplomasPaginatedResponseVM(
                    result.Data.Data,
                    result.Data.Page,
                    result.Data.PerPage,
                    result.Data.Total,
                    result.Data.TotalPages
                );

                var finalResponse = ResponseViewModel<GetAllDiplomasPaginatedResponseVM>.Success(responseVM);

                return Results.Ok(finalResponse);
            })
            .WithTags("Diploma")
            .WithName("GetAllDiplomas");
        }
    }
}