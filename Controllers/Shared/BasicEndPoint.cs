namespace ExaminationSystem.Controllers.Shared
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BasicEndPoint<TRequest, TResponse> : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly IValidator<TRequest> _validator;
        public BasicEndPoint(EndPointBasicParameters<TRequest> parameters)
        {
            _mediator = parameters.Mediator;
            _validator = parameters.Validator;
        }

        protected ResponseViewModel<TResponse> ValidateVMRequest(TRequest request)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

                return ResponseViewModel<TResponse>.Failure(validationErrors, ResponseVmErrorCode.ValidationError);
            }
            return ResponseViewModel<TResponse>.Success(default(TResponse)!, "Validation Succeeded");
        }
    }
}
