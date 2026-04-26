
namespace ExaminationSystem.Features.QuizFeatures.AddQuestionsToQuiz.Commands
{
    public record AddQuestionsToQuizCommand(Guid QuizId, List<Guid> QuestionIds) : IRequest<RequestResult<bool>>;

    public class AddQuestionsToQuizCommandHandler : IRequestHandler<AddQuestionsToQuizCommand, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Quiz> _quizRepository;

        public AddQuestionsToQuizCommandHandler(IGeneralRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public Task<RequestResult<bool>> Handle(AddQuestionsToQuizCommand request, CancellationToken cancellationToken)
        {

            var Questions = _quizRepository
                .GetById(request.QuizId)
                .Select(q => q.Questions.Select(q => q.Id));

            foreach (var questionId in request.QuestionIds)
            {
                if (!Questions.Any(q => q.Contains(questionId)))
                {
                    var question = new Question { Id = questionId };
                    // _quizRepository.AddRange(request.QuestionIds);
                }
            }

            throw new NotImplementedException();
        }
    }
}
