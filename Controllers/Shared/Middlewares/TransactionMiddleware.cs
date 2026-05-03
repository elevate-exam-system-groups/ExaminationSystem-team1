namespace ExaminationSystem.Controllers.Shared.Middlewares
{
    public class TransactionMiddleware
    {
        RequestDelegate _next;
        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, Domain.Data.IUnitOfWork context)
        {
            if (httpContext.Request.Method == HttpMethods.Get)
            {
                await _next(httpContext);
            }

            var transaction = context.Database.BeginTransaction();

            try
            {
                await _next(httpContext);
                //await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
