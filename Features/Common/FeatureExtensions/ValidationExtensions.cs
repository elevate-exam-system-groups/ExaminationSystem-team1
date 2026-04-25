namespace ExaminationSystem.Features.Common.FeatureExtensions
{
    public static class ValidationExtensions
    {
        public static async Task<RequestResult<TResponse>> ValidateRequestAsync<TRequest, TResponse>(
        this IValidator<TRequest> validator,
        TRequest request,
        CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<TResponse>.Failure(errors, RequestErrorCode.ValidationError);
            }

            return RequestResult<TResponse>.Success(default!, "Validation Succeeded");
        }
    }
}
