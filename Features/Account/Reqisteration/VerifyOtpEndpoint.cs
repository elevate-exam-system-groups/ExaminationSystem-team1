using ExaminationSystem.Controllers.AccountController;
using ExaminationSystem.Features.Account.Reqisteration.Command;

namespace ExaminationSystem.Features.Account.Reqisteration
{
    public static class VerifyOtpEndpoint
    {
        public static void MapVerifyOtpEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v2/Auth/VerifyOtp", async (
                [FromBody] OtpRequestDto request,
                IMediator _mediator) =>
            {
                var result = await _mediator.Send(new VerifyOtpCommand(request.Email, request.OtpCode));
                
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<string>.Failure(result.Message, ResponseVmErrorCode.InvalidCredentials));
                }

                return Results.Ok(ResponseViewModel<string>.Success(result.Data));
            })
            .WithTags("Auth")
            .WithName("VerifyOtp");
        }
    }
}
