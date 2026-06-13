using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MediatR;
using System.Security.Claims;
using ExaminationSystem.Features.Authentication.Queries;
using ExaminationSystem.Features.Authentication.Commands;

namespace ExaminationSystem.Features.Authentication
{
    public static class AuthEndpoints
    {
        public record LoginRequest(string Email, string Password);
        public record RegisterRequest(string FullName, string Email, string Password, string PhoneNumber);

        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            // Login Endpoint (Validates credentials from DB using MediatR Query)
            app.MapPost("/api/auth/login", async (LoginRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(new LoginQuery(request.Email, request.Password));

                if (!result.IsSuccess)
                {
                    return Results.Json(new { Error = "Invalid credentials" }, statusCode: StatusCodes.Status401Unauthorized);
                }

                return Results.Ok(new
                {
                    Message = "Login successful",
                    UserId = result.UserId,
                    Username = result.Username,
                    Email = result.Email,
                    Role = result.Role
                });
            });

            // Register Endpoint (Registers student in DB using MediatR Command)
            app.MapPost("/api/auth/register", async (RegisterRequest request, IMediator mediator) =>
            {
                var command = new RegisterStudentCommand(request.FullName, request.Email, request.Password, request.PhoneNumber);
                var result = await mediator.Send(command);

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { Error = result.Message });
                }

                return Results.Ok(new
                {
                    Message = result.Message,
                    UserId = result.UserId
                });
            });
        }
    }
}
