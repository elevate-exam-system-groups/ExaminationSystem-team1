using ExaminationSystem.Domain.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExaminationSystem.Controllers.Shared.Middlewares
{
    public class TransactionMiddleware
    {
        RequestDelegate _next;
        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, Context context)
        {
            if (!httpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                IDbContextTransaction transaction = default;
                try
                {
                    transaction = await context.Database.BeginTransactionAsync();
                    await _next(httpContext);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
