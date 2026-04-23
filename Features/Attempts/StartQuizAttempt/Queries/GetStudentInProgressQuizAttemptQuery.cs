namespace ExaminationSystem.Features.Attempts.StartQuizAttempt.Queries
{
    public record GetStudentInProgressQuizAttemptQuery(string StudentId, Guid quizId)
        : IRequest<RequestResult<Guid?>>;

    public class GetStudentInProgressQuizAttemptQueryHandler
        : IRequestHandler<GetStudentInProgressQuizAttemptQuery, RequestResult<Guid?>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptRepository;
        public GetStudentInProgressQuizAttemptQueryHandler(IGeneralRepository<QuizAttempt> quizAttemptRepository)
        {
            _quizAttemptRepository = quizAttemptRepository;
        }
        public async Task<RequestResult<Guid?>> Handle(GetStudentInProgressQuizAttemptQuery request, CancellationToken cancellationToken)
        {
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


