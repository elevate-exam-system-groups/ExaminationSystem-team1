
using ExaminationSystem.Features.StartQuizAttempt.Queries.GetQuizAttemptMetaData.DTOS;

namespace ExaminationSystem.Features.StartQuizAttempt.Queries.GetQuizAttemptMetaData
{
    public record GetQuizMetaDataQuery(Guid QuizId)
        : IRequest<RequestResult<QuizMetaDataDTO>>;

    public class GetQuizAttemptMetaDataHandler
        : IRequestHandler<GetQuizMetaDataQuery, RequestResult<QuizMetaDataDTO>>
    {
        private readonly IGeneralRepository<Quiz> _quizRepository;
        public GetQuizAttemptMetaDataHandler(IGeneralRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }
        public async Task<RequestResult<QuizMetaDataDTO>> Handle(GetQuizMetaDataQuery request, CancellationToken cancellationToken)
        {

            var quiz = await _quizRepository
                .Get(qa => qa.Id == request.QuizId)
                .Select(q => new QuizMetaDataDTO
                (
                  q.DiplomaId,
                  q.Title,
                  q.Instructions,
                  q.PassScore,
                  q.Questions
                   .OrderBy(qq => qq.OrderIndex)
                   .Select(qq => new QuestionDTO
                     (
                       qq.Id,
                       qq.Text,
                       qq.OrderIndex,
                       qq.Options
                         .Select(o => new OptionDTO(o.Id, o.Text))
                         .ToList()
                      ))
                    .ToList()
                 ))
                .FirstOrDefaultAsync(cancellationToken);

            if (quiz is null)
            {
                return RequestResult<QuizMetaDataDTO>
                    .Failure("Quiz not found", RequestErrorCode.NotFound);
            }

            return RequestResult<QuizMetaDataDTO>.Success(quiz);
        }
    }


}
