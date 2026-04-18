namespace ExaminationSystem.Features.Common
{
    public abstract class BaseRequestHandler<TRequest, TResponse>

        : IRequestHandler<TRequest, RequestResult<TResponse>>
        where TRequest : IRequest<RequestResult<TResponse>>

    {
        protected IMediator _mediator;
        protected IValidator<TRequest> _validator;
        public BaseRequestHandler(HandlerBasicParameterss<TRequest> parameters)
        {
            _mediator = parameters.Mediator;
            _validator = parameters.Validator;
        }

        public abstract Task<RequestResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);

        protected RequestResult<TResponse> ValidateRequest(TRequest request)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

                return RequestResult<TResponse>.Failure(validationErrors, RequestErrorCode.ValidationError);
            }
            return RequestResult<TResponse>.Success(default(TResponse)!, "Validation Succeeded");
        }

    }
}
