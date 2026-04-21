using ExaminationSystem.Domain.Data;

namespace ExaminationSystem.Controllers.Shared.Middlewares
{
    public class TransactionMiddleware
    {

        private readonly RequestDelegate _next;
        public TransactionMiddleware(RequestDelegate next)
            => _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (!httpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                var dbContext = httpContext.RequestServices.GetRequiredService<Context>();
                await using var transaction = await dbContext.Database.BeginTransactionAsync();

                try
                {
                    await _next(httpContext);
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