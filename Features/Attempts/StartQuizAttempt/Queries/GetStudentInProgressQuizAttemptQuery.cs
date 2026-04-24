namespace ExaminationSystem.Features.Attempts.StartQuizAttempt.Queries
{
    public record GetStudentInProgressQuizAttemptQuery(string StudentId, Guid quizId)
        : IRequest<RequestResult<Guid?>>;


    public class GetStudentInProgressQuizAttemptQueryValidator
    : AbstractValidator<GetStudentInProgressQuizAttemptQuery>
    {
        public GetStudentInProgressQuizAttemptQueryValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Student ID is required");
            RuleFor(x => x.quizId)
                .NotEmpty().WithMessage("Quiz ID is required");
        }
    }

    public class GetStudentInProgressQuizAttemptQueryHandler
        : IRequestHandler<GetStudentInProgressQuizAttemptQuery, RequestResult<Guid?>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptRepository;
        private readonly IValidator<GetStudentInProgressQuizAttemptQuery> _validator;
        public GetStudentInProgressQuizAttemptQueryHandler(IGeneralRepository<QuizAttempt> quizAttemptRepository, IValidator<GetStudentInProgressQuizAttemptQuery> validator)
        {
            _quizAttemptRepository = quizAttemptRepository;
            _validator = validator;
        }
        public async Task<RequestResult<Guid?>> Handle(GetStudentInProgressQuizAttemptQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
             .ValidateRequestAsync<GetStudentInProgressQuizAttemptQuery, Guid?>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var result = await _quizAttemptRepository
                .Get(qa => qa.StudentId == request.StudentId
                    && qa.QuizId == request.quizId
                    && qa.Status == QuizAttemptStatus.InProgress)
                .Select(qa => qa.Id)
                .FirstOrDefaultAsync(cancellationToken);

            return RequestResult<Guid?>.Success(result);
        }
    }


}


