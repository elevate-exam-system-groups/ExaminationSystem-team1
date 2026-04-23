using ExaminationSystem.Features.Attempts.StartQuizAttempt.Queries.GetQuizAttemptMetaData.DTOS;

namespace ExaminationSystem.Features.Attempts.StartQuizAttempt.Queries.GetQuizAttemptMetaData
{
    public record GetQuizAttemptMetaData (Guid AttemptId) 
        : IRequest<RequestResult<AttemptMetaDataDTO>>;

    public class GetQuizAttemptMetaDataHandler
        :IRequestHandler<GetQuizAttemptMetaData, RequestResult<AttemptMetaDataDTO>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptRepository;
        public GetQuizAttemptMetaDataHandler(IGeneralRepository<QuizAttempt> quizAttemptRepository)
        {
            _quizAttemptRepository = quizAttemptRepository;
        }
        public async Task<RequestResult<AttemptMetaDataDTO>> Handle(GetQuizAttemptMetaData request, CancellationToken cancellationToken)
        {
            var attempt = _quizAttemptRepository
                .Get(qa => qa.Id == request.AttemptId);
                
            if (attempt == null)
            {
                return RequestResult<AttemptMetaDataDTO>
                    .Failure("Quiz Attempt not found", RequestErrorCode.NotFound);
            }
            var attemptMetaData = new AttemptMetaDataDTO
            (
                attempt.QuizId,
                attempt.StudentId,
                attempt.StartTime,

            );
            return RequestResult<AttemptMetaDataDTO>.Success(attemptMetaData);
        }
    }


}
