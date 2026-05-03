using ExaminationSystem.Controllers.AccountController;
using ExaminationSystem.Features.Account.Reqisteration.Command;

namespace ExaminationSystem.Features.Account.Reqisteration
{
    public static class ResendOtpEndpoint
    {
        public static void MapResendOtpEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v2/Auth/ResendOtp", async (
                [FromBody] ResendOtpRequestDto request,
                IMediator _mediator) =>
            {
                var result = await _mediator.Send(new ResendOtpCommand(request.Email));
                
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<string>.Failure(result.Message, ResponseVmErrorCode.InternalServerError));
                }

                return Results.Ok(ResponseViewModel<string>.Success(result.Data));
            })
            .WithTags("Auth")
            .WithName("ResendOtp");
        }
    }
}
