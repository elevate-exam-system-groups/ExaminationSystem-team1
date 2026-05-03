using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.DiplomaFeatures.SharedDiploma.Queries.GetDiplomaEnrollments.DTOS;

namespace ExaminationSystem.Features.DiplomaFeatures.SharedDiploma.Queries.GetDiplomaEnrollments
{
    public record GetDiplomaEnrollmentsqueryRequest(Guid DiplomaId)
        : IRequest<RequestResult<List<DiplomaEnrollmentsDto>>>;

    public class GetDiplomaEnrollmentsqueryRequestValidator : AbstractValidator<GetDiplomaEnrollmentsqueryRequest>
    {
        public GetDiplomaEnrollmentsqueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }
    public class GetDiplomaEnrollmentsqueryRequestHandler : IRequestHandler<GetDiplomaEnrollmentsqueryRequest, RequestResult<List<DiplomaEnrollmentsDto>>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IValidator<GetDiplomaEnrollmentsqueryRequest> _validator;
        public GetDiplomaEnrollmentsqueryRequestHandler(IGeneralRepository<Enrollment> enrollmentRepository, IValidator<GetDiplomaEnrollmentsqueryRequest> validator)
        {
            _enrollmentRepository = enrollmentRepository;
            _validator = validator;
        }
        public async Task<RequestResult<List<DiplomaEnrollmentsDto>>> Handle(GetDiplomaEnrollmentsqueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ",
                                            validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<List<DiplomaEnrollmentsDto>>
                       .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            var enrollments = _enrollmentRepository
                .Get(e => e.DiplomaId == request.DiplomaId)
                .Select(e => new DiplomaEnrollmentsDto
                {
                    Id = e.Id,
                    DiplomaId = e.DiplomaId,
                    StudentId = e.StudentId,
                    EnrolledAt = e.CreatedAt
                }).ToList();

            return RequestResult<List<DiplomaEnrollmentsDto>>.Success(enrollments);
        }
    }
}
