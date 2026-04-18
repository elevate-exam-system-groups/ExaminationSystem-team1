using ExaminationSystem.Domain.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExaminationSystem.Controllers.Shared.Middlewares
{
    public class TransactionMiddleware
    {
        Context _context;
        public TransactionMiddleware(Context context)
        {
            _context = context;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            if (!httpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                IDbContextTransaction transaction = default;

                try
                {
                    transaction = _context.Database.BeginTransaction();

                    await next(httpContext);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }
    }
}
