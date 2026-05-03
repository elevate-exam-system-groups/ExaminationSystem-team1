using ExaminationSystem.Domain.Data;
using System.Security.Claims;

namespace ExaminationSystem.Controllers.Shared.Middlewares
{
    public class UpdateLastActivityMiddleware
    {

        private readonly RequestDelegate _next;
        public UpdateLastActivityMiddleware(RequestDelegate next)
            => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(userId))
                {
                    var dbContext = context.RequestServices.GetRequiredService<Context>();

                    await dbContext.Users
                        .Where(u => u.Id == userId)
                        .Where(u => u.LastActivityAt == null ||
                                    EF.Functions.DateDiffMinute(u.LastActivityAt, DateTime.UtcNow) >= 5)
                        .ExecuteUpdateAsync(s => s.SetProperty(u => u.LastActivityAt, DateTime.UtcNow));
                }
            }

            await _next(context);
        }
    }
}