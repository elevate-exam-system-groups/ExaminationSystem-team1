using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MediatR;
using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ExaminationSystem.Domain.Data;
using ExaminationSystem.Features.Authentication.Queries;
using ExaminationSystem.Features.Authentication.Commands;
using ExaminationSystem.Infrastructure.Authentication;

namespace ExaminationSystem.Features.Authentication
{
    public static class AuthEndpoints
    {
        public record LoginRequest(string Username, string Password);
        public record RegisterRequest(string FullName, string Email, string Password, string PhoneNumber);

        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            // Login Endpoint (Validates credentials from DB directly and generates JWT)
            app.MapPost("/api/auth/login", async (LoginRequest request, Context dbContext, IJwtTokenGenerator jwtTokenGenerator) =>
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return Results.Json(new { Error = "Invalid Credentials" }, statusCode: StatusCodes.Status401Unauthorized);
                }

                // Fetch the user from dbContext.Users matching Username or Email
                var user = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.UserName == request.Username || u.Email == request.Username);

                if (user == null)
                {
                    return Results.Json(new { Error = "Invalid Credentials" }, statusCode: StatusCodes.Status401Unauthorized);
                }

                // Verify the password using BCrypt
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
                if (!isPasswordValid)
                {
                    return Results.Json(new { Error = "Invalid Credentials" }, statusCode: StatusCodes.Status401Unauthorized);
                }

                // Dynamically determine user role by checking Admins and Students tables
                string role = "Student"; // Default role
                bool isAdmin = await dbContext.Admins.AnyAsync(a => a.UserId == user.Id);
                if (isAdmin)
                {
                    role = "Admin";
                }
                else
                {
                    bool isStudent = await dbContext.Students.AnyAsync(s => s.UserId == user.Id);
                    if (!isStudent)
                    {
                        if (user.Email.Contains("admin", StringComparison.OrdinalIgnoreCase))
                        {
                            role = "Admin";
                        }
                    }
                }

                // Generate JWT Access Token
                var token = jwtTokenGenerator.GenerateToken(user, role);

                return Results.Ok(new
                {
                    AccessToken = token
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
