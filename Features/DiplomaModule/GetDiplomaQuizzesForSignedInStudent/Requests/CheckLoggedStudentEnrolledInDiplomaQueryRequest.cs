namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.Requests
{
    public record CheckLoggedStudentEnrolledInDiplomaQueryRequest(Guid DiplomaId, string StudentId)
        : IRequest<RequestResult<bool>>;

    public class CheckLoggedStudentEnrolledInDiplomaQueryRequestValidator
        : AbstractValidator<CheckLoggedStudentEnrolledInDiplomaQueryRequest>
    {
        public CheckLoggedStudentEnrolledInDiplomaQueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");

            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required");
        }
    }

    public class CheckLoggedStudentEnrolledInDiplomaQueryHandler
        : IRequestHandler<CheckLoggedStudentEnrolledInDiplomaQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Enrollment> _EnrollmentRepository;
        private readonly IValidator<CheckLoggedStudentEnrolledInDiplomaQueryRequest> _validator;
        public CheckLoggedStudentEnrolledInDiplomaQueryHandler(IGeneralRepository<Enrollment> EnrollmentRepository,
            IValidator<CheckLoggedStudentEnrolledInDiplomaQueryRequest> validator)
        {
            _EnrollmentRepository = EnrollmentRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckLoggedStudentEnrolledInDiplomaQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ",
                    validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<bool>.Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            var isEnrolled = _EnrollmentRepository.Get(e =>
                e.DiplomaId == request.DiplomaId && e.StudentId == request.StudentId).Any();

            if (!isEnrolled)
            {
                return RequestResult<bool>
                    .Failure("Student is not enrolled in the diploma", RequestErrorCode.Forbidden);
            }

            return RequestResult<bool>.Success(true);
        }
    }
}
