using AutoMapper;
using ExaminationSystem.Controllers.DiplomaController.ViewModels;
using ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Orchestrators;

namespace ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent
{
    public static class GetDiplomaQuizzesEndpoint
    {
        public static void MapGetDiplomaQuizzesEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v2/Diploma/GetDiplomaQuizzesForLoggedStudent", async (
                [FromQuery] Guid diplomaRequestID, 
                [FromQuery] string StudentId,
                IMediator _mediator,
                IMapper _mapper) =>
            {
                var requestResponse = await _mediator
                    .Send(new GetDiplomaQuizzesForLoggedStudentOrchestrator(diplomaRequestID, StudentId));

                if (!requestResponse.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<List<GetDiplomaQuizzesForLoggedStudenResponseVM>>
                        .Failure(requestResponse.Message, (ResponseVmErrorCode?)requestResponse.requestErrorCode));
                }

                var responseVM = _mapper
                    .Map<List<GetDiplomaQuizzesForLoggedStudenResponseVM>>(requestResponse.Data);

                return Results.Ok(ResponseViewModel<List<GetDiplomaQuizzesForLoggedStudenResponseVM>>.Success(responseVM));
            })
            .WithTags("Diploma")
            .WithName("GetDiplomaQuizzesForLoggedStudent");
        }
    }
}
