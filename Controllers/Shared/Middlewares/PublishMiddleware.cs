
namespace ExaminationSystem.Controllers.Shared.Middlewares
{
    public class PublishMiddleware : IMiddleware
    {
        private readonly IMediator _mediator;

        public PublishMiddleware(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);
            throw new NotImplementedException();
        }
    }
}
