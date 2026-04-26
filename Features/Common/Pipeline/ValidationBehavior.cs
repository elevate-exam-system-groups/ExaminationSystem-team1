
namespace ExaminationSystem.Features.Common.Pipeline
{
    public class ValidationBehavior<TRequest, TResponse>
     : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
             TRequest request,
             RequestHandlerDelegate<TResponse> next,
             CancellationToken cancellationToken)
        {

            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();

            if (failures.Any())
            {
                var errorMessage = string.Join(", ", failures.Select(f => f.ErrorMessage));
                var genericType = typeof(TResponse).GetGenericArguments()[0];

                //var failureResult = RequestResult<object>.Failure(
                //    errorMessage,
                //    RequestErrorCode.ValidationError
                //);

                var failureResult = typeof(RequestResult<>)
                    .MakeGenericType(genericType)
                    .GetMethod("Failure")!
                    .Invoke(null, new object[] { errorMessage, RequestErrorCode.ValidationError });
                //return (TResponse)(object)failureResult;
                return (TResponse)failureResult!;
            }

            return await next();
        }
    }
}
