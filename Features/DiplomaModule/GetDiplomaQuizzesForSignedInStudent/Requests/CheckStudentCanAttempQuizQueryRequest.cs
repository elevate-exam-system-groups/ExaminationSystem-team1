namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.Requests
{
    public record CheckStudentCanAttempQuizQueryRequest(Guid quizId, string StudentId) : IRequest<RequestResult<bool>>;


    public class CheckStudentCanAttempQuizQueryRequestHandler
       : IRequestHandler<CheckStudentCanAttempQuizQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;

        public CheckStudentCanAttempQuizQueryRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
        }

        public async Task<RequestResult<bool>> Handle(CheckStudentCanAttempQuizQueryRequest request, CancellationToken cancellationToken)
        {
            var maxAttempt = _quizAttemptsRepository
                .Get(qa => qa.QuizId == request.quizId)
                 .Select(q => new
                 {
                     attempts = q.Quiz.MaxAttempts
                 }).FirstOrDefault();

            var canAttempt = _quizAttemptsRepository
                .Get(qa => qa.StudentId == request.StudentId && qa.QuizId == request.quizId)
                .Count() < maxAttempt?.attempts;

            if (!canAttempt)
            {
                return RequestResult<bool>.Success(false);
            }

            return RequestResult<bool>.Success(true);
        }
    }


}
