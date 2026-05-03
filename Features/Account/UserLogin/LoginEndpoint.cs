using ExaminationSystem.Controllers.AccountController.ViewModels.LoginViewModels;
using ExaminationSystem.Features.Account.UserLogin.Command;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Features.Account.UserLogin
{
    public static class LoginEndpoint
    {
        public static void MapLoginEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v2/Auth/Login", async (
                [FromBody] LoginRequestVm loginModel,
                IMediator _mediator,
                HttpContext context) =>
            {
                var result = await _mediator.Send(new LoginCommandRequest(loginModel.Email, loginModel.Password));

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(ResponseViewModel<UserResponseVm>
                        .Failure(result.Message ?? "Invalid Login", ResponseVmErrorCode.InvalidCredentials));
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(7),
                    Secure = true,   // Ensure HTTPS
                    SameSite = SameSiteMode.Strict
                };

                context.Response.Cookies.Append("refreshToken", result.Data.RefreshToken, cookieOptions);

                var responseVm = new UserResponseVm
                {
                    Email = result.Data.Email,
                    Token = result.Data.Token
                };

                return Results.Ok(ResponseViewModel<UserResponseVm>.Success(responseVm));
            })
            .WithTags("Auth")
            .WithName("Login");
        }
    }
}
