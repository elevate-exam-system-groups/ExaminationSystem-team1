using ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetDiplomaQuizzes.DTOS;

namespace ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetDiplomaQuizzes
{
    public record GetDiplomaQuizzesQueryRequest(Guid DiplomaId)
        : IRequest<RequestResult<List<GetDiplomaQuizzesDTO>>>;


    public class GetDiplomaQuizzesQueryRequestValidator
    : AbstractValidator<GetDiplomaQuizzesQueryRequest>
    {
        public GetDiplomaQuizzesQueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class GetDiplomaQuizzesForLoggedStudentQueryRequestHandler
    : IRequestHandler<GetDiplomaQuizzesQueryRequest, RequestResult<List<GetDiplomaQuizzesDTO>>>
    {
        private readonly IGeneralRepository<Quiz> _quizRepository;
        private readonly IValidator<GetDiplomaQuizzesQueryRequest> _validator;

        public GetDiplomaQuizzesForLoggedStudentQueryRequestHandler(IGeneralRepository<Quiz> quizRepository, IValidator<GetDiplomaQuizzesQueryRequest> validator)
        {
            _quizRepository = quizRepository;
            _validator = validator;
        }

        public async Task<RequestResult<List<GetDiplomaQuizzesDTO>>> Handle(GetDiplomaQuizzesQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
               .ValidateRequestAsync<GetDiplomaQuizzesQueryRequest,
                List<GetDiplomaQuizzesDTO>>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var Diplomaquizzes = await _quizRepository
                .Get(q => q.DiplomaId == request.DiplomaId && q.Status == QuizStatus.Published)
                .Select(q => new GetDiplomaQuizzesDTO
                (q.Id,
                q.Title,
                q.DurationInMinutes,
                q.PassScore,
                q.MaxAttempts,
                q.Status
                )).ToListAsync(cancellationToken);

            if (Diplomaquizzes == null || !Diplomaquizzes.Any())
            {
                return RequestResult<List<GetDiplomaQuizzesDTO>>
                .Failure("No quizzes found for the specified diploma");
            }

            return RequestResult<List<GetDiplomaQuizzesDTO>>
                .Success(Diplomaquizzes);
        }
    }
}
