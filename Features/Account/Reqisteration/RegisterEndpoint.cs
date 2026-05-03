using ExaminationSystem.Features.Account.Reqisteration.Command;
using ExaminationSystem.Features.Account.Reqisteration.DTOs;

namespace ExaminationSystem.Features.Account.Reqisteration
{
    public static class RegisterEndpoint
    {
        public static void MapRegisterEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v2/Auth/Register", async (
                [FromBody] RegisterDTO registerDTO,
                IMediator _mediator) =>
            {
                var result = await _mediator.Send(new RegisterCommand(registerDTO));
                
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<string>.Failure(result.Message, ResponseVmErrorCode.InternalServerError));
                }

                return Results.Ok(ResponseViewModel<string>.Success(result.Data));
            })
            .WithTags("Auth")
            .WithName("Register");
        }
    }
}
